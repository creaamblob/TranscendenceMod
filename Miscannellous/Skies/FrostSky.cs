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

            if (maxDepth >= 0 && minDepth < 0)
            {
                Texture2D gradient = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Gradient").Value;

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null);

                spriteBatch.Draw(gradient, new Rectangle(Main.screenWidth / 2, Main.screenHeight, Main.screenWidth, Main.screenHeight * 2), null, Color.Blue * 0.375f * fadeIn, 0, gradient.Size() * 0.5f, SpriteEffects.FlipVertically, 0);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null);
            }
        }
    }
}


