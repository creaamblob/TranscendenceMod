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


namespace TranscendenceMod.Miscannellous.UI
{
    public class ShieldParryBar : UIElement
    {
        public bool Hovering => false;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ShieldParryBar").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ShieldParryBar_Jewel").Value;

            Player player = Main.LocalPlayer;

            Vector2 pos = player.Center - Main.screenPosition;
            bool Hover = Main.MouseWorld.Between(pos + Main.screenPosition + new Vector2(-sprite.Width / 2, 50), pos + Main.screenPosition + new Vector2(sprite.Width / 2, 50 + sprite.Height / 2f));

            bool onCD = player.GetModPlayer<TranscendencePlayer>().ParryTimerCD < player.GetModPlayer<TranscendencePlayer>().ParryCD;
            int timer = (int)(player.GetModPlayer<TranscendencePlayer>().ParryTimer);

            if (player != null && player.GetModPlayer<NucleusGame>().Active)
            {
                Main.mouseText = false;
                Main.hoverItemName = "";
                Main.isMouseLeftConsumedByUI = true;
                Main.LocalPlayer.mouseInterface = true;
            }

            if (player != null && player.active && player.GetModPlayer<TranscendencePlayer>().Parry > 0)
            {
                int x = (int)pos.X;
                int y = (int)(pos.Y + 5 + player.gfxOffY);

                int width = (int)MathHelper.Lerp(0, 52, timer / (float)player.GetModPlayer<TranscendencePlayer>().ParryAmount);
                float cd = player.GetModPlayer<TranscendencePlayer>().ParryTimerCD < 0 ? -1f : 1;
                int width2 = (int)MathHelper.Lerp(0, 52, (player.GetModPlayer<TranscendencePlayer>().ParryTimerCD * cd) / (float)player.GetModPlayer<TranscendencePlayer>().ParryCD);

                Rectangle rec = new Rectangle(x - (sprite.Width / 2), y + 45, sprite.Width, sprite.Height);
                Rectangle rec2 = new Rectangle(x - 26, y + 53, onCD ? width2 : width, 8);

                Color c = player.GetModPlayer<TranscendencePlayer>().BrokenShield ? Color.Red : onCD ? Color.White : timer < 5 ? Color.Lime : Color.Red;

                spriteBatch.Draw(sprite, rec, player.GetModPlayer<TranscendencePlayer>().BrokenShield ? Color.DarkRed : Color.White);
                spriteBatch.Draw(sprite2, rec, c);

                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec2, c);

                bool showParticles = timer > 0 || onCD || player.GetModPlayer<TranscendencePlayer>().BrokenShield;
                if (player.GetModPlayer<TranscendencePlayer>().ParryTimerCD > 1 && showParticles)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2(x - 26 + (onCD ? width2 : width), y + 57) + Main.rand.NextVector2Circular(4f, 8f), null, c, 0f, TextureAssets.BlackTile.Value.Size() * 0.5f, 0.175f, SpriteEffects.None, 0f);
                    }
                }

                if (Hover)
                {
                    Main.hoverItemName = Language.GetTextValue("Mods.TranscendenceMod.Messages.ParryUI");

                    Main.LocalPlayer.mouseInterface = true;
                    Main.mouseText = true;

                }
            }
        }
    }

    public class ParryFocusState : UIState
    {
        public ShieldParryBar parry;
        public FocusGauge focus;

        public override void OnInitialize()
        {
            parry = new ShieldParryBar();
            Append(parry);

            focus = new FocusGauge();
            Append(focus);
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI
{
    [Autoload(Side = ModSide.Client)]
    public class ParryBarDraw : ModSystem
    {
        internal ParryFocusState parryBar;
        private UserInterface parryBar2;

        public override void Load()
        {
            parryBar = new ParryFocusState();
            parryBar.Activate();

            parryBar2 = new UserInterface();
            parryBar2.SetState(parryBar);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            parryBar2?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Parry and Focus Gauges", delegate
                {
                    parryBar2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}
