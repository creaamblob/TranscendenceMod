using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace TranscendenceMod.Miscannellous.UI
{
    public class ChargerCharge : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ChargerCharge").Value;
            Player player = Main.LocalPlayer;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (player != null && player.active && player.HeldItem.TryGetGlobalItem(out ModifiersItem modiers) && modiers.Modifier == ModifierIDs.Charger)
            {
                int x = (int)(player.Center.X - Main.screenPosition.X);
                int y = (int)(player.Center.Y - Main.screenPosition.Y);

                float charge = player.HeldItem.GetGlobalItem<ModifiersItem>().ChargerCharge;
                int width = (int)MathHelper.Lerp(0f, 32f, charge - 0.25f);
                Rectangle rec = new Rectangle(x - (sprite.Width / 2), y + 35, sprite.Width, sprite.Height);
                Rectangle rec2 = new Rectangle(x - 16, y + 41, width, 4);

                spriteBatch.Draw(sprite, rec, Color.White);
                spriteBatch.Draw(TextureAssets.BlackTile.Value, rec2, charge < 0.66f ? Color.Red : new Color(10, 207, 232));
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);
        }
    }

    public class ChargerUIState : UIState
    {
        public ChargerCharge charger;
        public override void OnInitialize()
        {
            charger = new ChargerCharge();
            Append(charger);
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI
{
    [Autoload(Side = ModSide.Client)]
    public class ChargerUIDraw : ModSystem
    {
        internal ChargerUIState chargers;
        private UserInterface chargers2;

        public override void Load()
        {
            chargers = new ChargerUIState();
            chargers.Activate();

            chargers2 = new UserInterface();
            chargers2.SetState(chargers);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            chargers2?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Charger Modifier UI", delegate
                {
                    chargers2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}
