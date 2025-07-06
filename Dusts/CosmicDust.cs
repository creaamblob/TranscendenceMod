using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class CosmicDust : ModDust
    {
        public float size = 1;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override bool Update(Dust dust)
        {
            return base.Update(dust);
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphFontShader").Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size());
            eff.Parameters["uTime"].SetValue(0f);
            eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly);

            eff.Parameters["useAlpha"].SetValue(false);
            eff.Parameters["useExtraCol"].SetValue(false);

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            for (int i = 0; i < 3; i++)
                sb.Draw(sprite, dust.position - Main.screenPosition, null, Color.White, dust.rotation, origin, dust.scale / 3, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(sprite, dust.position - Main.screenPosition, null, Color.White, dust.rotation, origin, dust.scale * 0.166666666667f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
