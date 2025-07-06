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

            Vector2 pos = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);

            bool onCD = player.GetModPlayer<TranscendencePlayer>().ParryTimerCD < player.GetModPlayer<TranscendencePlayer>().ParryCD;
            int timer = (int)(player.GetModPlayer<TranscendencePlayer>().ParryTimer);

            if (player != null && player.GetModPlayer<NucleusGame>().Active)
            {
                Main.mouseText = false;
                Main.hoverItemName = "";
                Main.isMouseLeftConsumedByUI = true;
                Main.LocalPlayer.mouseInterface = true;
            }

            if (player != null && player.active && player.TryGetModPlayer(out TranscendencePlayer modPlayer) && modPlayer.HasParry && modPlayer.CanParry())
            {
                int x = (int)pos.X;
                int y = (int)(pos.Y + (55 + player.gfxOffY) * Main.UIScale);


                int width = (int)MathHelper.Lerp(0, 52, timer / (float)modPlayer.ParryAmount);
                float cd = modPlayer.ParryTimerCD < 0 ? -1f : 1;
                int width2 = (int)MathHelper.Lerp(0, 52, (modPlayer.ParryTimerCD * cd) / (float)modPlayer.ParryCD);
                bool noFocus = modPlayer.Focus < modPlayer.ParryFocusCost;

                x -= sprite.Width / 2;
                Rectangle rec = new Rectangle(x, y, sprite.Width, sprite.Height);
                Rectangle rec2 = new Rectangle(x + 16, y + 8, onCD ? width2 : width, 8);

                Color c = noFocus ? new Color(50, 50, 50) : onCD ? Color.White : timer <= 5 ? Color.Lime : Color.Red;

                spriteBatch.Draw(sprite, rec, noFocus ? new Color(100, 100, 100) : Color.White);
                spriteBatch.Draw(sprite2, rec, c);

                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec2, c);

                bool Hover = Main.MouseWorld.Between(new Vector2(x, y) + Main.screenPosition, new Vector2(x, y) + Main.screenPosition + new Vector2(sprite.Width, sprite.Height));

                bool showParticles = timer > 0 || onCD || modPlayer.BrokenShield;
                if (modPlayer.ParryTimerCD > 1 && showParticles)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2(x + 16 + (onCD ? width2 : width), y + 12) + Main.rand.NextVector2Circular(4f, 8f), null, c, 0f, TextureAssets.BlackTile.Value.Size() * 0.5f, 0.175f, SpriteEffects.None, 0f);
                    }
                }

                if (Hover)
                {
                    Main.hoverItemName = Language.GetTextValue("Mods.TranscendenceMod.Messages.ParryUI");
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
