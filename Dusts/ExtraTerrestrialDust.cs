using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class ExtraTerrestrialDust : ModDust
    {
        public override Color? GetAlpha(Dust dust, Color lightColor) => Color.White;
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            sb.Draw(sprite, dust.position - Main.screenPosition, null, new Color(0.3f, 0.1f, 0.5f, 0f), 0, origin, dust.scale / 6f, SpriteEffects.None, 0);

            return true;
        }
    }
}
