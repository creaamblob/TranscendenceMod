using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs
{
    public class HolyFireball : ModProjectile
    {
        public int DustCh;
        public float FadeToBlack;
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
            Projectile.timeLeft = 360;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }
        public override Color? GetAlpha(Color lightColor) => Color.Lerp(Color.White, Color.Black, FadeToBlack);
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            FadeToBlack = 0f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Vector2 origin = sprite.Size() * 0.5f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 pos = Projectile.oldPos[i] - Main.screenPosition;
                float Fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                Main.EntitySpriteDraw(sprite, pos + Projectile.Size / 2, null, Color.DarkRed * Fade,
                Projectile.rotation, origin, Projectile.scale * Fade, SpriteEffects.None);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
            Color color0 = Color.Lerp(Color.White, Color.Red, FadeToBlack);
            Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, color0,
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);

            Color color = Color.Lerp(Color.Gold, Color.Red, FadeToBlack);

            TranscendenceUtils.DrawEntity(Projectile, color * 0.66f, 1, "bloom", 0, Projectile.Center + Projectile.velocity * 2, null);

            return false;
        }
        public override void AI()
        {
            if (++Timer % 8 == 0)
            {
                int d = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<HolyDust>(), 0, 0, 0, Color.White, 1f);
                Main.dust[d].velocity = Vector2.Zero;
            }

            if (Projectile.scale == 1 && (Main.getGoodWorld || Main.zenithWorld)) Projectile.velocity *= 1.01f;
            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.timeLeft < 60)
                FadeToBlack += 1 / 60f;
        }
    }
}