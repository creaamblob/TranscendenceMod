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
            Vector2 pos = player.Center + new Vector2(0, 5 / Main.UIScale) - Main.screenPosition;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);


            if (player != null && player.active && player.TryGetModPlayer(out TranscendencePlayer modPlayer) && modPlayer != null && modPlayer.HasParry && modPlayer.CanParry())
            {
                int x = (int)pos.X;
                int y = (int)(pos.Y + (75 + player.gfxOffY) * Main.UIScale);

                int width = (int)MathHelper.Lerp(0, 64, modPlayer.Focus / modPlayer.MaxFocus);
                int width2 = (int)MathHelper.Lerp(0, 64, modPlayer.ParryFocusCost / modPlayer.MaxFocus);

                x -= sprite.Width / 2;
                Rectangle rec = new Rectangle(x, y, sprite.Width, sprite.Height);
                Rectangle rec2 = new Rectangle(x + 8, y + 6, width, 6);
                Rectangle rec3 = new Rectangle(x + 8, y + 6, width2, 6);

                spriteBatch.Draw(sprite, rec, Color.White);

                if (modPlayer.Focus <= modPlayer.ParryFocusCost)
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, rec3, Color.DarkRed);

                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec2, Color.White);

                if (modPlayer.Focus >= modPlayer.ParryFocusCost)
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, rec3, Color.SpringGreen);

                bool Hover = Main.MouseWorld.Between(new Vector2(x, y) + Main.screenPosition, new Vector2(x, y) + Main.screenPosition + new Vector2(sprite.Width, sprite.Height));
                if (Hover)
                {
                    Main.hoverItemName = Language.GetTextValue("Mods.TranscendenceMod.Messages.FocusUI") + $" ({Math.Round(modPlayer.Focus, 0)}/{modPlayer.MaxFocus})";
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);
        }
    }
}
