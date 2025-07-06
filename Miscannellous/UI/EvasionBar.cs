using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TranscendenceMod.Miscannellous.UI
{
    public class EvasionBar : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/EvasionBar").Value;

            Texture2D watch = TextureAssets.Item[ItemID.GoldWatch].Value;
            Texture2D arrow = TextureAssets.Item[ItemID.HellfireArrow].Value;

            Player player = Main.LocalPlayer;

            if (player != null && player.active && player.TryGetModPlayer(out TranscendencePlayer modplayer) && modplayer.EvasionStoneEquipped && !modplayer.EvasionStoneExists)
            {
                int x = Main.screenWidth / 2;
                int y = Main.screenHeight / 2;

                int width = modplayer.EvasionStoneGraze == 0 ? 0 : (int)MathHelper.Lerp(0f, 28f, modplayer.EvasionStoneGraze / 13f);
                int width2 = modplayer.EvasionStoneTimer == 0 ? 0 : (int)MathHelper.Lerp(0f, 28f, modplayer.EvasionStoneTimer / (float)modplayer.EvasionStoneMaxTimer);

                Rectangle rec = new Rectangle(x - (sprite.Width / 2), y - 69, sprite.Width, sprite.Height);
                Rectangle rec2 = new Rectangle(x - (sprite.Width / 2), y - 54, sprite.Width, sprite.Height);

                //Amount itself
                Rectangle rec3 = new Rectangle(x - 14, y - 65, width, 6);
                Rectangle rec32 = new Rectangle(x - 20, y - 70, arrow.Width / 2, arrow.Height / 2);

                Rectangle rec4 = new Rectangle(x - 14, y - 50, width2, 6);
                Rectangle rec42 = new Rectangle(x - 24, y - 55, watch.Width / 2, watch.Height / 2);

                if (modplayer.EvasionStoneTimer > 0)
                {
                    spriteBatch.Draw(sprite, rec, Color.White);
                    spriteBatch.Draw(sprite, rec2, Color.White);

                    spriteBatch.Draw(arrow, rec32, Color.White);
                }

                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec3, Color.Red);
                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec4, Color.DeepPink);

                spriteBatch.Draw(watch, rec42, Color.White);
            }
        }
    }

    public class EvasionBarState : UIState
    {
        public EvasionBar state;
        public override void OnInitialize()
        {
            state = new EvasionBar();
            Append(state);
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI
{
    [Autoload(Side = ModSide.Client)]
    public class EvasionBarDraw : ModSystem
    {
        internal EvasionBarState stoneS;
        private UserInterface stoneS2;

        public override void Load()
        {
            stoneS = new EvasionBarState();
            stoneS.Activate();

            stoneS2 = new UserInterface();
            stoneS2.SetState(stoneS);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            stoneS2?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Evasion Stone Bars", delegate
                {
                    stoneS2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}
