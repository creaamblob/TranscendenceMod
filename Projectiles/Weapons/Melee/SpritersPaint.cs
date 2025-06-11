using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class SpritersPaint : ModProjectile
    {
        public int Bounce;
        public Color col;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.penetrate = 5;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = -Projectile.velocity.Y;
            if (++Bounce > 1)
                Projectile.velocity.Y = 0;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.Paint, Main.rand.NextVector2Circular(4, 4), 0, col, 2);
            }
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;

            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().PaintBounceCD == 0 && Main.LocalPlayer.Center.Between(Projectile.Center - new Vector2(30, 15), Projectile.Center + new Vector2(30, 5)) && Bounce > 1)
            {
                Main.LocalPlayer.velocity.Y -= 20;
                Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().PaintBounceCD = 15;
                for (int i = 0; i < 5; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Paint, Main.rand.NextVector2Circular(9, 9), 0, col, 2);
                    d.noGravity = true;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            col = Main.hslToRgb(Projectile.ai[1], 1f, 0.5f);

            sb.End();
            sb.Begin(default, BlendState.NonPremultiplied, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.velocity.Y != 0) TranscendenceUtils.DrawTrailProj(Projectile, col, 1f, Texture, true, false, 2f, Vector2.Zero);
            TranscendenceUtils.DrawEntity(Projectile, col, 1f, Texture, 0f, Projectile.Center, null);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}