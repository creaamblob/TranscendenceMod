using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class StellarFireball : ModProjectile
    {
        public int DustCh;
        public Vector2 SpawnPos;
        public float FadeToBlack;
        public float TelegraphFade = 0f;
        public int Timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.timeLeft = 600;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            SpawnPos = Projectile.Center;
            FadeToBlack = 0f;
            TelegraphFade = 0f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;


            if (Projectile.ai[2] == 1 && TelegraphFade > 0)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                Texture2D telegraph = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

                spriteBatch.Draw(telegraph, new Rectangle(
                    (int)(SpawnPos.X - Main.screenPosition.X),
                    (int)(SpawnPos.Y - Main.screenPosition.Y),
                    (int)MathHelper.Lerp(0, 25, TelegraphFade),
                    6000), null,
                    Color.Lerp(Color.DeepSkyBlue, Color.Purple, TelegraphFade) * TelegraphFade, Projectile.velocity.ToRotation() + MathHelper.PiOver2,
                    telegraph.Size() * 0.5f,
                    SpriteEffects.None,
                    0);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            int frameStuff = sprite.Height / Main.projFrames[Type];
            int y = frameStuff * Projectile.frame;
            Rectangle rec = new Rectangle(0, y, Projectile.width, frameStuff);
            Vector2 origin = rec.Size() * 0.5f;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 pos = Projectile.oldPos[i] - Main.screenPosition;
                float Fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;

                Main.EntitySpriteDraw(sprite, pos +Projectile.Size / 2, rec, Color.DarkBlue * Fade,
                Projectile.rotation, origin, Projectile.scale * Fade, SpriteEffects.None);
            }
            Color color0 = Color.Lerp(Color.White, Color.Red, FadeToBlack);

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, true);

            Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, rec, color0,
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);

            Color color = Color.Lerp(Color.DarkViolet, Color.Red, FadeToBlack);
            Color color2 = Color.Lerp(Color.DarkViolet, Color.Red, FadeToBlack);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.velocity != Vector2.Zero)
            {
                TranscendenceUtils.DrawEntity(Projectile, color2 * 0.65f, Projectile.scale * 0.4f, "TranscendenceMod/Miscannellous/Assets/Slash",
                    Projectile.rotation, Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver4) * 10, null);

                TranscendenceUtils.DrawEntity(Projectile, color2 * 0.65f, Projectile.scale * 0.3f, "TranscendenceMod/Miscannellous/Assets/Slash",
                    Projectile.rotation + MathHelper.PiOver2, Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver4) * 10, null);

                TranscendenceUtils.DrawEntity(Projectile, color2, 0.65f, "TranscendenceMod/Miscannellous/Assets/StarEffect", ++Projectile.localAI[2] / 15, Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver4) * 10, null);
                TranscendenceUtils.DrawEntity(Projectile, color0, 0.5f, "TranscendenceMod/Miscannellous/Assets/StarEffect", ++Projectile.localAI[2] / 15, Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver4) * 10, null);

            }

            TranscendenceUtils.DrawEntity(Projectile, color * 0.75f, Projectile.scale * 0.2f, "TranscendenceMod/Miscannellous/Assets/GlowBloom", 0, Projectile.Center + Projectile.velocity * 2, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.velocity == Vector2.Zero)
                Projectile.rotation = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().PreTimeStopVel.ToRotation();

            return false;
        }
        public override void AI()
        {
            if (++Timer % 3 == 0)
            {
                int d = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 3) * 15), 1, 1, ModContent.DustType<NovaDust>(),
                    0, 0, 0, Color.White, 1f);
                Main.dust[d].velocity = Vector2.Zero;

                int d2 = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2 - MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 3) * 15), 1, 1, ModContent.DustType<NovaDust>(),
                    0, 0, 0, Color.White, 1f);
                Main.dust[d2].velocity = Vector2.Zero;
            }

            if (TelegraphFade < 1 && Projectile.timeLeft > 550)
                TelegraphFade += 0.0175f;

            if (Projectile.timeLeft < 550 && TelegraphFade > 0)
                TelegraphFade -= 0.0175f;

            if (Projectile.scale == 1 && (Main.getGoodWorld || Main.zenithWorld)) Projectile.velocity *= 1.01f;
            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.timeLeft < 60)
                FadeToBlack += 1 / 60f;

            Main.projFrames[Projectile.type] = 3;
            TranscendenceUtils.AnimateProj(Projectile, 4);
        }
    }
}