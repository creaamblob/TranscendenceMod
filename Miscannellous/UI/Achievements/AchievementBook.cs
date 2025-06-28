using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscanellous.UI.Achievements.Tasks;
using TranscendenceMod.Miscannellous.UI.Achievements;

namespace TranscendenceMod.Miscannellous.UI.Achievements
{
    public class AchievementBookUI : UIState
    {
        UIImageButton achBook;
        public override void OnInitialize()
        {
            achBook = new UIImageButton(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Achievements/AchievementBook"));

            achBook.Width.Set(26f, 0f);
            achBook.Height.Set(38f, 0f);
            achBook.SetHoverImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Achievements/AchievementBook_Hover"));

            achBook.Left.Set(405f, 0f);
            achBook.Top.Set(265f, 0f);

            achBook.OnLeftClick += ModButtonExit_OnLeftClick;
            achBook.OnUpdate += AchBook_OnUpdate;

            Append(achBook);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (achBook.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.hoverItemName = Language.GetTextValue($"Mods.TranscendenceMod.Achievement.Title");
            }
        }

        private void AchBook_OnUpdate(UIElement affectedElement)
        {
            affectedElement.Left.Set(405f + ModContent.GetInstance<TranscendenceConfig>().AchievementBookOffsetX, 0f);
        }

        private void ModButtonExit_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            listeningElement.Height.Set(38f, 0f);

            QuestBookUIDrawing.Visible = !QuestBookUIDrawing.Visible;
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI.Achievements
{
    [Autoload(Side = ModSide.Client)]
    public class AchievementBookDrawing : ModSystem
    {
        internal AchievementBookUI mauis;
        private UserInterface mauis2;
        public bool Visible => Main.playerInventory;

        public override void Load()
        {
            mauis = new AchievementBookUI();
            mauis.Activate();

            mauis2 = new UserInterface();
            mauis2.SetState(mauis);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (Visible)
            {
                mauis?.Update(gameTime);
                mauis2?.Update(gameTime);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Achievement Book UI", delegate
                {
                    if (Visible) mauis2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}

