using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Miscannellous.Skies
{
    public class NullSky : ModSurfaceBackgroundStyle
    {
        private float fadeIn = 0;
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

            float fade = Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().NullFade;
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0.0125f, 0.0125f, 0.0125f, fade) * fade);
            spriteBatch.Draw(ModContent.Request<Texture2D>($"Terraria/Images/Misc/VortexSky/Background").Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0.75f, 0.75f, 0.75f) * 0.375f);

            return true;
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


