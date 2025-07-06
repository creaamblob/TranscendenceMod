using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Miscannellous.UI.ModifierUI;

namespace TranscendenceMod.Miscannellous.UI
{
    public class ModifierContainerUI : UIState
    {
        public override void OnInitialize()
        {
            UIPanel panel = new UIPanel();

            panel.Width.Set(318f, 0f);
            panel.Height.Set(189f, 0f);

            panel.Left.Set(-318f / 2f, 0.5f);
            panel.Top.Set(125f, 0.5f);

            panel.BackgroundColor = new Color(0.21f, 0.11f, 0.38f, 0.85f);
            panel.OnUpdate += TitlePanel_OnUpdate;

            Append(panel);

            int x = 0;
            int y = 0;
            int index = 0;

            for (int i = 0; i < 28; i++)
            {
                ModifierContainerUISlot slot = new ModifierContainerUISlot();

                slot.xMod = x;
                x += 42;

                if (index > 6)
                {
                    x = 42;
                    y += 42;
                    index = 0;
                }

                //Fix repeat Mondays overflowing
                if (i > 6 && index == 0)
                    slot.xMod = 0;

                index += 1;

                slot.yMod = y;
                slot.index = i;

                panel.Append(slot);
            }
        }

        private void TitlePanel_OnUpdate(UIElement affectedElement)
        {
            if (!ModifierContainerUIDrawing.Visible)
                return;

        }
    }
    public class ModifierContainerUISlot : UIElement
    {
        public int xMod = 0;
        public int yMod = 0;
        public static int x = 24;
        public static int y = 24;

        public Item Item2;
        public Item Item2Temp;
        public int CD;

        public int index;

        Rectangle rec = new Rectangle(x, y, 32, 32);
        public bool ShouldBeActive = false;
        public bool Hovering => Main.MouseWorld.Between(new Vector2(x + Main.screenPosition.X + xMod + 4, y + Main.screenPosition.Y + yMod + 4),
            new Vector2(x + Main.screenPosition.X + xMod + 38, y + Main.screenPosition.Y + yMod + 38));
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            x = (int)GetDimensions().X;
            y = (int)GetDimensions().Y;

            if (ShouldBeActive)
            {
                bool HasItem = Item2 != null && !Item2.IsAir;

                Texture2D bg = TextureAssets.InventoryBack.Value;

                Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer mp);

                if (mp.ModifierContainerItem.ModItem is ModifierContainer cont && cont.CurSelection == index && HasItem)
                    bg = TextureAssets.InventoryBack10.Value;


                rec = new Rectangle(x + xMod, y + yMod, 42, 42);

                spriteBatch.Draw(bg, rec, Hovering ? Color.White : new Color(150, 150, 150));

                if (Hovering)
                {
                    if (HasItem)
                    {
                        Main.HoverItem = Item2.Clone();
                        Main.hoverItemName = Item2.Clone().Name;
                        Main.mouseText = true;
                    }
                }

                if (HasItem)
                {
                    Vector2 pos = new Vector2(x + xMod + 21, y + yMod + 21);
                    ItemSlot.DrawItemIcon(Item2, ItemSlot.Context.InventoryItem, spriteBatch, pos, Hovering ? 1.25f : 1f, bg.Width, Color.White);
                }
            }
        }

        public void TakeItem(Item it)
        {
            it.SetDefaults(Item2.type);
            it.prefix = Item2.prefix;
            it.stack = Item2.stack;

            it.GetGlobalItem<ModifiersItem>().ModifiersUnlocked = Item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked;
            SoundEngine.PlaySound(SoundID.Research);

            if (Item2.GetGlobalItem<ModifiersItem>().Modifier > 0)
                it.GetGlobalItem<ModifiersItem>().Modifier = Item2.GetGlobalItem<ModifiersItem>().Modifier;

            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ModifierContainerItem.ModItem is ModifierContainer cont)
                cont.ItemsInside[index].TurnToAir();

            Item2.TurnToAir();
            CD = 0;
        }

        public override void Update(GameTime gameTime)
        {
            ShouldBeActive = true;
            Item item = Main.mouseItem;

            x = (int)GetDimensions().X;
            y = (int)GetDimensions().Y;

            if (!ModifierContainerUIDrawing.Visible)
                return;

            if (!Main.mouseLeft) CD++;

            if (!Main.playerInventory)
                ModifierContainerUIDrawing.Visible = false; 

            Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer mp);

            if (mp.ModifierContainerItem.ModItem is ModifierContainer cont)
            {
                bool HasItem = false;
                for (int i = 0; i < 10; i++)
                {
                    Item it = Main.LocalPlayer.inventory[i];
                    if (it != null && !it.IsAir && it == mp.ModifierContainerItem)
                        HasItem = true;
                }
                if (!HasItem)
                    ModifierContainerUIDrawing.Visible = false;

                Item2 = cont.ItemsInside[index];

                if (Item2 == null)
                {
                    Item2 = new Item();
                }

                if (Hovering)
                {
                    if (!Item2.IsAir && item.IsAir && CD > 15)
                    {
                        if (Main.mouseLeft)
                            TakeItem(item);
                        if (Main.mouseRight && cont.CurSelection != index)
                        {
                            cont.CurSelection = index;
                            SoundEngine.PlaySound(SoundID.Research);
                        }
                    }

                    Main.LocalPlayer.mouseInterface = true;

                    bool hasAlready = false;
                    for (int i = 0; i < cont.ItemsInside.Length; i++)
                    {
                        Item it = cont.ItemsInside[i];
                        if (it != null && !it.IsAir && it.type == item.type)
                            hasAlready = true;
                    }

                    if (Item2 != item && item.stack >= 3 && item != null && !hasAlready && item.ModItem is BaseModifier mod && (int)mod.ModifierType < 1000 && Main.mouseLeft && Item2.IsAir && !item.IsAir && CD > 15)
                    {
                        SoundEngine.PlaySound(SoundID.Research);

                        Item2 = item.Clone();
                        Item2Temp = Item2;

                        if (item.stack > 3)
                        {
                            Item2.stack = 3;
                            item.stack -= 3;
                        }
                        else
                        {
                            item.TurnToAir();
                        }

                        if (cont.ItemsInside[cont.CurSelection] == null || cont.ItemsInside[cont.CurSelection].IsAir)
                            cont.CurSelection = index;

                        cont.ItemsInside[index] = Item2;

                        CD = 0;
                    }
                }
            }
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI.ModifierUI
{
    [Autoload(Side = ModSide.Client)]
    public class ModifierContainerUIDrawing : ModSystem
    {
        internal ModifierContainerUI ui;
        private UserInterface ui2;
        public static bool Visible;
        public static int HideTimer;

        public override void Load()
        {
            ui = new ModifierContainerUI();
            ui.Activate();

            ui2 = new UserInterface();
            ui2.SetState(ui);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (HideTimer > 0)
                HideTimer--;

            if (Visible)
            {
                ui?.Update(gameTime);
                ui2?.Update(gameTime);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Modifier Container UI", delegate
                {
                    if (Visible) ui2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}