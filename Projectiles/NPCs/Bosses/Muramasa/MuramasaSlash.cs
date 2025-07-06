using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Muramasa
{
    public class MuramasaSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;

            Projectile.width = 32;
            Projectile.height = 34;

            Projectile.tileCollide = false;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
        }
        public override void AI()
        {
            int d = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 15f), 1, 1, ModContent.DustType<MuramasaDust>(),
                0, 0, 0, Color.White, 0.5f);
            Main.dust[d].velocity = Vector2.Zero;

            int d2 = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2 - MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 15f), 1, 1, ModContent.DustType<MuramasaDust>(),
                0, 0, 0, Color.White, 0.5f);
            Main.dust[d2].velocity = Vector2.Zero;

            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;

            Lighting.AddLight(Projectile.Center, 0, 0.4f, 0.8f);

            if (++Projectile.ai[2] > 45 )
            {
                Projectile.velocity *= 1.075f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            string sprite = Texture;
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.DeepSkyBlue, 1f, sprite, true, true, 2f, Vector2.Zero);
            TranscendenceUtils.DrawEntity(Projectile, Color.Aqua * 0.5f, Projectile.scale * 1.5f, sprite, Projectile.rotation, Projectile.Center, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, sprite, Projectile.rotation, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}