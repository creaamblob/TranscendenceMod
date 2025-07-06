using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class NucleusBlood : ModDust
    {
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;

            dust.velocity.Y += 0.150f;
            dust.scale *= 0.999f;
            dust.rotation += 0.01f;

            if (dust.scale < 0.05f)
                dust.active = false;
            return false;
        }
        

        public override bool PreDraw(Dust dust)
        {

            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/GlowBloomNoBG").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>(Texture).Value;

            sb.Draw(sprite, dust.position - Main.screenPosition, null, dust.color, dust.rotation, sprite.Size() * 0.5f, dust.scale * 0.1f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(sprite2, dust.position - Main.screenPosition, null, dust.color, dust.rotation, sprite2.Size() * 0.5f, dust.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
