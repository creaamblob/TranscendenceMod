using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Skies
{
    public class FrostSky : CustomSky
    {
        private bool active = false;
        private float fadeIn = 0;
        public float auroraTimer;
        public float auroraTurner = 0.0025f;

        public override void Activate(Vector2 position, params object[] args)
        {
            active = true;
        }
        public override void Deactivate(params object[] args)
        {
            active = false;
        }
        public override bool IsActive() => fadeIn > 0f;
        public override void Update(GameTime gameTime)
        {
            if (fadeIn < 1 && active)
                fadeIn += 0.025f;
            if (!active && fadeIn > 0)
                fadeIn -= 0.025f;

        }
        public override Color OnTileColor(Color inColor)
        {
            return Color.Lerp(inColor, Color.Lerp(Color.Blue, new Color(0, 70, 255), (float)Math.Sin(TranscendenceWorld.UniversalRotation)) * 0.25f, fadeIn - 0.5f);
        }
        public override void Reset() => active = false;
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            auroraTimer += auroraTurner;
            if (auroraTimer > 2 || auroraTimer < 0) auroraTurner = -auroraTurner;

            if (minDepth < maxDepth)
            {
                Texture2D gradient = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Gradient").Value;

                if (maxDepth >= float.MaxValue && minDepth < float.MaxValue)
                {
                    var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphTextShader", AssetRequestMode.ImmediateLoad).Value;
                    eff.Parameters["uColor"].SetValue(new Vector3(0f, 1f, 1f));
                    eff.Parameters["uOpacity"].SetValue(1f * fadeIn);
                    eff.Parameters["uImageSize0"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
                    eff.Parameters["uTime"].SetValue(auroraTimer * 0.075f);
                    eff.Parameters["xChange"].SetValue(Main.GlobalTimeWrappedHourly * 0.2f);
                    //Apply Space Textures
                    Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Perlin2").Value;
                    eff.Parameters["uImageSize1"].SetValue(new Vector2(shaderImage.Width, shaderImage.Height * 12));
                    Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.Additive, default, default, default, null);

                    spriteBatch.Draw(gradient, new Rectangle(0, (int)(Main.screenHeight * 0.3625f), Main.screenWidth * 2, (int)(Main.screenHeight * 0.75f)), null, Color.DeepSkyBlue * 0.75f * fadeIn, 0, gradient.Size() * 0.5f, SpriteEffects.FlipVertically, 0);
                    spriteBatch.Draw(gradient, new Rectangle(0, (int)(Main.screenHeight * 0.9f), Main.screenWidth * 2, (int)(Main.screenHeight * 0.328f)), null, Color.DeepSkyBlue * 0.75f * fadeIn, 0, gradient.Size() * 0.5f, SpriteEffects.None, 0);

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null);
                }
            }
        }
    }
}


