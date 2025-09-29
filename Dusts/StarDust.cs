using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class StarDust : ModDust
    {
        public static int Timer;
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 22, 22);
            dust.noGravity = true;
            dust.velocity *= 2;
            dust.scale *= 0.1f;
            dust.alpha = 0;

            if (dust.color == default)
                dust.color = Color.White;
        }
        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, 0.25f, 0.25f, 0.25f);
            dust.velocity *= 0.975f;
            dust.position += dust.velocity;

            if (++dust.alpha > 30)
            {
                dust.scale *= 0.975f;
                if (dust.scale < 0.02f)
                    dust.active = false;
            }
            else dust.scale *= 1.1f;

            return false;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor) => dust.color;
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/StarEffect").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            sb.Draw(sprite, dust.position + new Vector2(11 * 0.66f * dust.scale) - Main.screenPosition, null, dust.color * 0.5f, 0, origin, dust.scale / 3, SpriteEffects.None, 0);
            sb.Draw(sprite2, dust.position + new Vector2(11 * 0.66f * dust.scale) - Main.screenPosition, null, Color.White * 0.75f, 0, sprite2.Size() * 0.5f, dust.scale, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
