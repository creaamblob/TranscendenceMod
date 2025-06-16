using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class PoCStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;

            Projectile.timeLeft = 300;
            Projectile.penetrate = 15;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 10, ModContent.DustType<ArenaDust>(), 4, Color.White, 1.25f);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 10, ModContent.DustType<StarDust>(), 4, Color.White, 1.25f);
            return base.PreKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.White * 0.75f, Projectile.scale * 2f, "TranscendenceMod/Miscannellous/Assets/StarEffect",
                Projectile.rotation, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.White * 0.66f, Projectile.scale, "bloom", 0, Projectile.Center, null);

            for (int i = 0; i < 4; i++)
            {
                float pi = MathHelper.TwoPi * i / 4;
                float rot = pi += MathHelper.ToRadians(++Projectile.localAI[2] + 4);

                Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(rot) * 5;

                TranscendenceUtils.DrawProjAnimated(Projectile, Color.DeepSkyBlue * 0.35f, Projectile.scale,
                    $"{Texture}", Projectile.rotation, pos, false, false, false);
            }

            return base.PreDraw(ref lightColor);
        }
    }
}