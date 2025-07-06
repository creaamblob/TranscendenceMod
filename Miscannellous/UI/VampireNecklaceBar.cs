using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace TranscendenceMod.Miscannellous.UI
{
    public class VampireNecklaceBar : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/VampireNecklaceBar").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/VampireNecklaceBar_Inner").Value;
            Player player = Main.LocalPlayer;

            if (player != null && player.active && player.TryGetModPlayer(out TranscendencePlayer modplayer) && modplayer.Vampire)
            {
                int x = Main.screenWidth / 2;
                int y = Main.screenHeight / 2 - 140;

                int width = (int)MathHelper.Lerp(0f, 32f, modplayer.VampireBlood / (float)modplayer.CrimsonNecklaceMaxBlood);
                Rectangle rec = new Rectangle(x - (sprite.Width / 2), y + 59, sprite.Width, sprite.Height);
                Rectangle rec2 = new Rectangle(x - 16, y + 65, width, 6);

                spriteBatch.Draw(sprite2, rec, Color.White);
                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec2, modplayer.VampireBlood >= modplayer.CrimsonNecklaceMaxBlood ? Color.White : Color.Red);
                spriteBatch.Draw(sprite, rec, Color.White);
            }
        }
    }

    public class VampireNecklaceBarState : UIState
    {
        public VampireNecklaceBar neck;
        public override void OnInitialize()
        {
            neck = new VampireNecklaceBar();
            Append(neck);
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI
{
    [Autoload(Side = ModSide.Client)]
    public class VampireNecklaceBarDraw : ModSystem
    {
        internal VampireNecklaceBarState necks;
        private UserInterface necks2;

        public override void Load()
        {
            necks = new VampireNecklaceBarState();
            necks.Activate();

            necks2 = new UserInterface();
            necks2.SetState(necks);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            necks2?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Vampire Necklace UI", delegate
                {
                    necks2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}
