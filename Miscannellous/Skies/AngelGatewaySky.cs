using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Skies
{
    public class AngelGatewaySky : CustomSky
    {
        private bool active = false;
        private float fadeIn = 0;
        public static float Progress;

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
        public override void Reset() => active = false;
        public override float GetCloudAlpha() => 1f;
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/AngelGatewayBGShader").Value;
                if (eff != null)
                {
                    eff.Parameters["uColor"].SetValue(Color.Black.ToVector3());
                    eff.Parameters["uSecondaryColor"].SetValue(TranscendenceWorld.CosmicPurple2.ToVector3());
                    eff.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 4f);
                    eff.Parameters["centre"].SetValue(new Vector2(0.5f, 0.25f));
                    eff.Parameters["uProgress"].SetValue(Progress);


                    Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack").Value;
                    eff.Parameters["uImageSize1"].SetValue(new Vector2(Main.screenWidth, Main.screenWidth));
                    eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 4f);
                    Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                    spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenWidth), Color.Black);


                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.Additive, default, default, default, eff);

                    spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenWidth), Color.Black * 0.25f);

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null);
                }
            }
        }
    }
}


