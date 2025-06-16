using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Miscannellous.Skies
{
    public class SpaceSky : ModSurfaceBackgroundStyle
    {
        private float fadeIn = 0;
        private int perlinScroll = 10;
        private float starsRot;

        private float CosmosColorFadeTimer;
        private float CosmosColorFade = 0.01f;
        private Color CosmicPurple;

        private float BlackFadeTimer;
        private float BlackFade = 0.0075f;
        private Color Black;
        public override void Load()
        {
            fadeIn = 0;
        }
        public override void Unload()
        {
            fadeIn = 0;
        }

        public override bool PreDrawCloseBackground(SpriteBatch spriteBatch)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()))
                return false;

            starsRot += MathHelper.ToRadians(0.05f);

            CosmosColorFadeTimer += CosmosColorFade;
            if (CosmosColorFadeTimer > 1 || CosmosColorFadeTimer < 0) CosmosColorFade = -CosmosColorFade;
            CosmicPurple = Color.Lerp(Color.DeepSkyBlue, Color.Magenta, CosmosColorFadeTimer);

            float fade = Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().StarFade;
            Texture2D stars = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphStars").Value;

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/OrionNebula").Value;

            spriteBatch.Draw(sprite, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * fade);

            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * 0.75f * fade);

            //Draw the vignette
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphSkyShader", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uColor"].SetValue(new Vector3(0.25f, 0, 0.5f) * fade);

            eff.Parameters["uOpacity"].SetValue(8f * fade);
            eff.Parameters["uImageSize0"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f);
            eff.Parameters["uTime"].SetValue(-TranscendenceWorld.UniversalRotation * -0.025f);
            //Apply Space Textures
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/RainbowShader").Value;
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 2f);
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff);

            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive);

            for (int e = 0; e < 15; e++)
            {
                spriteBatch.Draw(stars, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), null, Color.White * 0.3f * (1f - (e / 15f)) * fade, -starsRot + MathHelper.ToRadians(e) - MathHelper.PiOver4, stars.Size() * 0.5f, 0.75f, SpriteEffects.None, 0);
            }

            spriteBatch.Draw(stars, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), null, Color.White * fade, -starsRot, stars.Size() * 0.5f, 1f, SpriteEffects.None, 0);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);

            return false;
        }
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    } 
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }
    }
}


