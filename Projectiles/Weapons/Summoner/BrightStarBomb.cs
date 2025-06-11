using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class BrightStarBomb : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public int Radians = 140;
        public float Distance = 1.001f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = -1;

            Projectile.timeLeft = 1200;
            Projectile.extraUpdates = 4;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 20, ModContent.DustType<ArenaDust>(), 4, Color.DeepSkyBlue, 1);

            /*for (int i = 0; i < 12; i++)
            {
                Vector2 fullCircle = Vector2.One.RotatedBy(MathHelper.ToRadians(0 + (i * 8)));
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center + fullCircle * 250, -fullCircle,
                    ModContent.ProjectileType<Yummy>(), (int)(Projectile.damage * 0.6f), 3, Main.player[Projectile.owner].whoAmI);
            }*/

            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            if (++Projectile.ai[1] > 35)
            {
                Projectile.friendly = true;
                Projectile.penetrate = 1;
            }
            Projectile.velocity.Y += 0.033f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, new Color(5, 25, 40), 1, "TranscendenceMod/Miscannellous/Assets/DashBall",
                MathHelper.ToRadians(Projectile.localAI[2] * 3), Projectile.Center, null);

            TranscendenceUtils.DrawEntity(Projectile, new Color(0, 10, 80), 0.75f, "TranscendenceMod/Miscannellous/Assets/DashBall",
                MathHelper.ToRadians(Projectile.localAI[2] * 3), Projectile.Center, null);


            TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.5f, "TranscendenceMod/Miscannellous/Assets/DashBall",
                MathHelper.ToRadians(++Projectile.localAI[2] * 3), Projectile.Center, null);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}