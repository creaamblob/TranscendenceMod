using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Skies;

namespace TranscendenceMod.Miscannellous.UI
{
    public class SpaceMenu : ModMenu
    {
        public override string DisplayName => "Seraphic";
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<TitleMenuSeraph>();
        public override int Music => MusicLoader.GetMusicSlot("TranscendenceMod/Miscannellous/Assets/Sounds/Music/CosmicFieldNight");
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/Logo");
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            drawColor = Color.White;
            return base.PreDrawLogo(spriteBatch, ref logoDrawCenter, ref logoRotation, ref logoScale, ref drawColor);
        }
    }
}


