using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscannellous.UI.Processer;

namespace TranscendenceMod.Miscannellous.UI.Processer.RecipeButtons
{
    public abstract class BaseRecipeButton : UIElement
    {
        public abstract int ItemType { get; }

        public int x;
        public int y;
        public abstract int xMod { get; }
        public abstract int yMod { get; }

        public abstract int resultAmount { get; }

        public abstract void OnClick();

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            x = (int)GetDimensions().X + 20;
            y = (int)GetDimensions().Y + 20;

            Vector2 pos = new Vector2(x + xMod, y + yMod + 40);
            bool Hovering = Main.MouseWorld.Distance(Main.screenPosition + pos) < 18;

            // Draw Panel
            Texture2D sprite = TextureAssets.InventoryBack.Value;
            spriteBatch.Draw(sprite, pos, null, Hovering ? Color.White : new Color(150, 150, 150), 0f, sprite.Size() * 0.5f, 1f * Main.UIScale, SpriteEffects.None, 0f);

            // Draw Icon
            Texture2D Icon = TextureAssets.Item[ItemType].Value;
            spriteBatch.Draw(Icon, pos, null, Color.White, 0f, Icon.Size() * 0.5f, (Hovering ? 1.25f : 1f) * Main.UIScale, SpriteEffects.None, 0f);


            string stack = resultAmount.ToString();
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value,
                stack, new Vector2(x + 10 + xMod, y + 44 + yMod), Color.White, 0, Vector2.Zero, Vector2.One);

            // Hover Text
            if (Hovering)
            {
                Item it = new Item(ItemType);

                UICommon.TooltipMouseText(
                    it.Name + "\n" +
                    Language.GetTextValue($"Mods.TranscendenceMod.Messages.RecipeBookClick"));

                Main.LocalPlayer.mouseInterface = true;
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    if (!Main.dedServ)
                        SoundEngine.PlaySound(SoundID.Mech with { MaxInstances = 0});
                    OnClick();
                }
            }
        }
    }
}