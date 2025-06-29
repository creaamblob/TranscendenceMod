using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using System.Net.Http;
using System.Net;
using TranscendenceMod.Miscanellous.MiscSystems;
using Terraria.Localization;
using System;


namespace TranscendenceMod.Miscannellous.UI
{
    public class FocusGauge : UIElement
    {
        public bool Hovering => false;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/FocusGauge").Value;

            Player player = Main.LocalPlayer;

            Vector2 pos = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f + 5f * Main.UIScale);

            if (player != null && player.active && player.TryGetModPlayer(out TranscendencePlayer modPlayer) && modPlayer != null && modPlayer.HasParry)
            {
                int x = (int)pos.X;
                int y = (int)(pos.Y + (75 + player.gfxOffY) * Main.UIScale);

                int width = (int)MathHelper.Lerp(0, 64, modPlayer.Focus / modPlayer.MaxFocus);

                x -= sprite.Width / 2;
                Rectangle rec = new Rectangle(x, y, sprite.Width, sprite.Height);
                Rectangle rec2 = new Rectangle(x + 8, y + 6, width, 6);

                spriteBatch.Draw(sprite, rec, Color.White);
                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec2, Color.White);

                bool Hover = Main.MouseWorld.Between(new Vector2(x, y) + Main.screenPosition, new Vector2(x, y) + Main.screenPosition + new Vector2(sprite.Width, sprite.Height));
                if (Hover)
                {
                    Main.hoverItemName = Language.GetTextValue("Mods.TranscendenceMod.Messages.FocusUI") + $" ({Math.Round(modPlayer.Focus, 0)}/{modPlayer.MaxFocus})";
                }
            }
        }
    }
}
