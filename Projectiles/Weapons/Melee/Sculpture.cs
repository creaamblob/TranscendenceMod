using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class Sculpture : ModProjectile
    {
        public Vector2 Position = Vector2.Zero;
        public int MinDistanceFromMouse = 15;
        private Texture2D sprite2;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;

            Projectile.penetrate = 4;
            Projectile.timeLeft = 65;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];

            Position = Projectile.velocity;
            Projectile.velocity = Vector2.Zero;
            Projectile.frame = Main.rand.Next(0, 4);
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.noEnchantmentVisuals = Projectile.localAI[2] < 30;
            Projectile.rotation = Position.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4;

            if (++Projectile.localAI[2] == 30)
            {
                Projectile.velocity = Position;
            }
            if (Projectile.timeLeft < 20)
                Projectile.scale -= 1f / 20f;
        }
        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Clay, Main.rand.NextVector2Circular(5, 5));
            }
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return base.PreKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile != null && projectile.active && projectile.Center.Distance(Projectile.Center) < 1000
                    && projectile.type == Type && (Projectile.ai[1] == projectile.ai[1] + 1 || projectile.ai[1] == 11 && Projectile.ai[1] == 0) && projectile.ai[2] == Projectile.ai[2])
                {
                    Vector2 pos2 = Projectile.Center - Main.screenPosition;
                    Rectangle rec = new Rectangle((int)pos2.X, (int)pos2.Y, (int)(10 * Projectile.scale), (int)Projectile.Distance(projectile.Center) * 2);

                    if (sprite2 == null)
                        sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine2").Value;

                    Main.spriteBatch.Draw(sprite2, rec, null, new Color(0.7f, 0.2f, 0f, 0f), Projectile.DirectionTo(projectile.Center).ToRotation() - MathHelper.PiOver2, default, default, 0);
                }
            }
            TranscendenceUtils.DrawProjAnimated(Projectile, lightColor, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, false, false, false);
            return false;
        }
    }
}