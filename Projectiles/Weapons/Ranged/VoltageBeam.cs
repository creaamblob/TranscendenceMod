using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class VoltageBeam : ModProjectile
    {
        public float rot;
        public Player player;
        public Vector2 Pos;
        public float lenght = 2000;
        private Texture2D sprite;
        public Vector2 endPos;
        public Color color;
        public int CollisionUpdateTimer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2500;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.ownerHitCheck = true;
 
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.localNPCHitCooldown = 3;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            

            rot = player.DirectionTo(Main.MouseWorld).ToRotation();
            Projectile.Center = player.Center + Vector2.One.RotatedBy(rot - MathHelper.PiOver4) * 30;

            if (player.GetModPlayer<TranscendencePlayer>().VoltageBeamTimer > 0 && !player.dead)
                Projectile.timeLeft = 5;

            if (CollisionUpdateTimer > 0)
                CollisionUpdateTimer--;
            else endPos = Vector2.Zero;
            for (int i = 0; i < 50; i++)
            {
                Vector2 dir = Vector2.One.RotatedBy(rot - MathHelper.PiOver2 + MathHelper.PiOver4);
                Vector2 pos3 = Projectile.Center + (dir * 22) * i;

                if (Collision.SolidCollision(pos3, 16, 16) && endPos == Vector2.Zero && CollisionUpdateTimer == 0)
                {
                    endPos = pos3;
                    Dust.NewDust(pos3, 1, 1, ModContent.DustType<MuramasaDust>(), Main.rand.Next(-6, 6), -15, 0, Color.Blue);
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            endPos = target.Center;
            CollisionUpdateTimer = 5;
            for (int i = 0; i < 5; i++) Dust.NewDust(endPos, 1, 1, ModContent.DustType<MuramasaDust>(), Main.rand.Next(-6, 6), -15, 0, Color.Blue);
        }
        public override void OnSpawn(IEntitySource source)
        {
            player = Main.player[Projectile.owner];
            Projectile.velocity = Vector2.Zero;

            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Vector2.One.RotatedBy(rot - MathHelper.PiOver2 + MathHelper.PiOver4) * lenght, 4, ref reference))
                return targetHitbox.Distance(Projectile.Center) < endPos.Distance(Projectile.Center);

            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.projFrames[Type] = 2;
            TranscendenceUtils.AnimateProj(Projectile, 3);

            if (sprite == null)
                sprite = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 pos = Projectile.Center - Main.screenPosition;
            SpriteBatch spriteBatch = Main.spriteBatch;
            int amount = 50;
            Vector2 dir = Vector2.One.RotatedBy(rot - MathHelper.PiOver2 + MathHelper.PiOver4);

            for (float f = 0; f < amount; f++)
            {
                Vector2 pos2 = Vector2.Lerp(pos, pos + dir * 32, f * 0.67f);
                int x = (int)pos2.X;
                int y = (int)pos2.Y;

                int frameSizeY = sprite.Height / Main.projFrames[Projectile.type];
                int defaultPos = frameSizeY * Projectile.frame;
                Rectangle rec = new Rectangle(0, defaultPos, sprite.Width, frameSizeY);
                Vector2 origin = rec.Size() * 0.5f;

                if ((pos2 + Main.screenPosition).Distance(Projectile.Center) < endPos.Distance(Projectile.Center) && Projectile.timeLeft < 25)
                    spriteBatch.Draw(sprite, new Rectangle(x, y, 16, 32), rec, Color.White, rot + MathHelper.PiOver2, origin, SpriteEffects.None, 0);
                //else if (collid && !collid2) Dust.NewDust(pos3, 1, 1, ModContent.DustType<MuramasaDust>(), Main.rand.Next(-6, 6), -15, 0,Color.Blue);
            }

            if (endPos != Vector2.Zero)
            {
                for (int i = 1; i < 5; i++)
                    TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.25f + (i / 2f), "bloom", 0, endPos, null);
            }
            return false;
        }
    }
}