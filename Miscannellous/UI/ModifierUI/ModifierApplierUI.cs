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
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Items.Weapons.Melee;

namespace TranscendenceMod.Miscannellous.UI.ModifierUI
{
    public class ModifierApplierUIState : UIState
    {
        public ModifiableItemSlot moddable;
        public ModifierItemSlot mod;
        public ModifierResourceSlot modItem;
        public ModifierResultSlot resultItem;

        //Added for the sake of localization
        //Language.GetTextValue() doesn't work in OnIntialize()
        public UIText text;
        public UIText priceText;
        public UIText priceTextCarbon;
        public UIImage arrow;
        public override void OnInitialize()
        {
            UIPanel modPanel = new UIPanel();

            modPanel.Width.Set(580f, 0f);
            modPanel.Height.Set(250f, 0f);

            modPanel.Left.Set(650f, 0f);
            modPanel.Top.Set(-275f, 0.5f);

            modPanel.BackgroundColor = new Color(0.05f, 0.15f, 0.3f, 0.85f);
            modPanel.OnUpdate += Panel_OnUpdate;

            Append(modPanel);

            UIPanel titlePanel = new UIPanel();

            titlePanel.Width.Set(605f, 0f);
            titlePanel.Height.Set(30f, 0f);

            titlePanel.Left.Set(0f, 0f);
            titlePanel.Top.Set(-125f, 0.5f);

            titlePanel.BackgroundColor = new Color(0.21f, 0.11f, 0.38f, 0.85f);
            titlePanel.OnUpdate += TitlePanel_OnUpdate;

            modPanel.Append(titlePanel);



            UIPanel pricePanel = new UIPanel();

            pricePanel.Width.Set(325f, 0f);
            pricePanel.Height.Set(125f, 0f);

            pricePanel.HAlign = 0.125f;
            pricePanel.VAlign = 0.45f;

            pricePanel.BackgroundColor = new Color(0.21f, 0.11f, 0.38f, 0.85f);
            pricePanel.OnUpdate += PricePanel_OnUpdate;

            modPanel.Append(pricePanel);



            text = new UIText("Apply Modifiers");
            text.HAlign = 0.5f;
            text.VAlign = 0.5f;
            titlePanel.Append(text);

            priceText = new UIText("Price");
            priceText.HAlign = 0.125f;
            priceText.VAlign = 0.5f;
            priceText.OnUpdate += PriceText_OnUpdate;
            pricePanel.Append(priceText);

            priceTextCarbon = new UIText("Price");
            priceTextCarbon.HAlign = 0.125f;
            priceTextCarbon.VAlign = 0.6f;
            priceTextCarbon.OnUpdate += PriceTextCarbon_OnUpdate;
            pricePanel.Append(priceTextCarbon);

            UIImageButton modButtonExit = new UIImageButton(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIClose"));

            modButtonExit.Width.Set(18f, 0f);
            modButtonExit.Height.Set(18f, 0f);
            modButtonExit.SetHoverImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIClose_Hover"));

            modButtonExit.Left.Set(490f, 0f);
            modButtonExit.Top.Set(-90f, 0.5f);

            modButtonExit.OnLeftClick += Button_OnLeftClick;

            modPanel.Append(modButtonExit);

            UIImageButton modButtonCombine = new UIImageButton(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierApplier"));

            modButtonCombine.Width.Set(108f, 0f);
            modButtonCombine.Height.Set(116f, 0f);
            modButtonCombine.SetHoverImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierApplier_Hover"));

            modButtonCombine.Left.Set(426f, 0f);
            modButtonCombine.Top.Set(0f, 0.33f);

            modButtonCombine.OnLeftClick += Button2_OnLeftClick;

            modPanel.Append(modButtonCombine);

            moddable = new ModifiableItemSlot();
            moddable.HAlign = 0.1f;
            moddable.VAlign = 0.33f;
            modPanel.Append(moddable);

            mod = new ModifierItemSlot();
            mod.HAlign = 0.1f;
            mod.VAlign = 0.33f;
            modPanel.Append(mod);

            modItem = new ModifierResourceSlot();
            moddable.HAlign = 0.1f;
            modItem.VAlign = 0.33f;
            modPanel.Append(modItem);

            arrow = new UIImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIArrow").Value);
            arrow.HAlign = 0.1f;
            arrow.VAlign = 0.33f;
            modPanel.Append(arrow);

            resultItem = new ModifierResultSlot();
            resultItem.HAlign = 0.1f;
            resultItem.VAlign = 0.33f;
            modPanel.Append(resultItem);
        }

        private void PricePanel_OnUpdate(UIElement affectedElement)
        {
            affectedElement.VAlign = 0.85f;
            priceText.VAlign = 0.33f;
            affectedElement.Width.Set(400, 0);
        }

        private void PriceTextCarbon_OnUpdate(UIElement affectedElement)
        {
            Item Item2 = moddable.Item2;

            if (Item2 == null || Item2.IsAir)
            {
                priceTextCarbon.SetText("");
                return;
            }

            string carbonString = $"[c/2b4670:" + ValueCarbon().ToString() + " " + Language.GetTextValue("Mods.TranscendenceMod.Items.CarbonOre.DisplayName") + "] " + $"[i:TranscendenceMod/CarbonOre]";
            priceTextCarbon.SetText(carbonString);
        }

        public float ValueCoins()
        {
            Player local = Main.LocalPlayer;
            NPC npc = local.GetModPlayer<TranscendencePlayer>().ModifierUINPCPos;

            if (moddable.Item2 == null || moddable.Item2.IsAir || moddable.Item2.value == 0)
                return 5f;

            float amount = moddable.Item2.value;
            if (IsACraftingRecipe(moddable.Item2, mod.Item2, modItem.Item2, moddable.Item2.type, mod.Item2.type, modItem.Item2.type, modItem.Item2.stack))
            {
                if (mod.Item2.ModItem is BaseModifier || mod.Item2.ModItem is ModifierContainer)
                {
                    BaseModifier it2 = null;

                    if (mod.Item2.ModItem is BaseModifier modifier)
                        it2 = modifier;

                    if (mod.Item2.ModItem is ModifierContainer cont)
                    {
                        Item it = cont.ItemsInside[cont.CurSelection];
                        if (it != null && !it.IsAir && it.ModItem is BaseModifier mod2)
                            it2 = mod2;
                    }

                    Item fakeItem = new Item();

                    bool actualRecipe = it2.CraftingResultItem > 0;
                    int type = actualRecipe ? it2.CraftingResultItem : mod.Item2.type;

                    fakeItem.type = type;
                    fakeItem.CloneDefaults(type);
                    fakeItem.SetDefaults(type);

                    float mult = 1.5f;
                    amount += fakeItem.value * mult;
                }
            }
            amount *= 0.5f;

            if (moddable.Item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked)
                amount *= 0.25f;

            amount *= local.GetModPlayer<TranscendencePlayer>().TinkererHappiness;
            return amount;
        }
        public int ValueCarbon()
        {
            if (moddable.Item2 == null || moddable.Item2.IsAir || moddable.Item2.value == 0)
                return 5;

            //Exponentially decreases, making the cost of cheap and expensive items not too far from each other
            float valueIncrease = MathHelper.Lerp(1250f, 37500f, moddable.Item2.value / 3250000f);
            float amount = moddable.Item2.value / valueIncrease;

            if (moddable.Item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked)
                amount *= 0.25f;

            int carbonAmount = (int)amount;

            if (carbonAmount < 5)
                carbonAmount = 5;

            return carbonAmount;
        }

        private void PriceText_OnUpdate(UIElement affectedElement)
        {
            Item Item2 = moddable.Item2;
            if (Item2 == null || Item2.IsAir)
            {
                priceText.SetText(Language.GetTextValue("Mods.TranscendenceMod.Messages.ModifierUIEmptyPricePanel"));
                return;
            }

            string text2 = "";
            int num70 = 0;
            int num72 = 0;
            int num73 = 0;
            int num74 = 0;
            int num75 = (int)ValueCoins();
            if (num75 < 1)
            {
                num75 = 1;
            }
            if (num75 >= 1000000)
            {
                num70 = num75 / 1000000;
                num75 -= num70 * 1000000;
            }
            if (num75 >= 10000)
            {
                num72 = num75 / 10000;
                num75 -= num72 * 10000;
            }
            if (num75 >= 100)
            {
                num73 = num75 / 100;
                num75 -= num73 * 100;
            }
            if (num75 >= 1)
            {
                num74 = num75;
            }
            if (num70 > 0)
            {
                text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + num70 + " " + Lang.inter[15].Value + "] ";
            }
            if (num72 > 0)
            {
                text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + num72 + " " + Lang.inter[16].Value + "] ";
            }
            if (num73 > 0)
            {
                text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + num73 + " " + Lang.inter[17].Value + "] ";
            }
            if (num74 > 0)
            {
                text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + num74 + " " + Lang.inter[18].Value + "] ";
            }
            string valueStr = text2 + " +";
            priceText.SetText(valueStr);
        }

        private void TitlePanel_OnUpdate(UIElement affectedElement)
        {
            if (!ModifierApplierUIDrawing.Visible)
                return;
            text.SetText(Language.GetTextValue("Mods.TranscendenceMod.Messages.ModifierUIButton"));
        }

        private void Panel_OnUpdate(UIElement affectedElement)
        {
            if (!ModifierApplierUIDrawing.Visible)
                return;

            Main.playerInventory = true;
            if (affectedElement.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            moddable.HAlign = 0.1f;
            mod.HAlign = 0.1f;
            modItem.HAlign = 0.1f;
            arrow.HAlign = 0.385f;
            arrow.VAlign = 0.175f;
            resultItem.HAlign = 0.2f;

            if (!CanApply() && resultItem.Item2 != null)
            {
                arrow.SetImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIArrow_Fail").Value);
                resultItem.Item2.TurnToAir();
                resultItem.Item2 = null;
            }
            else
            {
                arrow.SetImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIArrow").Value);
                bool cannotAffordCoins = false;
                bool cannotAffordCarbon = false;

                if (!Main.LocalPlayer.CanAfford((long)ValueCoins()))
                {
                    arrow.SetImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIArrow_TooPoor").Value);
                    cannotAffordCoins = true;
                }

                if (Main.LocalPlayer.CountItem(ModContent.ItemType<CarbonOre>()) < ValueCarbon())
                {
                    arrow.SetImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIArrow_NotEnoughCarbon").Value);
                    cannotAffordCarbon = true;
                }

                if (cannotAffordCoins && cannotAffordCarbon)
                    arrow.SetImage(ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierUIArrow_Nothing").Value);

                //Display Slot Item, NOT THE APPLY CODE !!
                if (moddable.Item2 != null && !moddable.Item2.IsAir && mod.Item2 != null && !mod.Item2.IsAir && mod.Item2.ModItem is BaseModifier modifier)
                {
                    Player local = Main.LocalPlayer;
                    NPC npc = local.GetModPlayer<TranscendencePlayer>().ModifierUINPCPos;

                    Item item2 = new Item();

                    if (IsACraftingRecipe(moddable.Item2, mod.Item2, modItem.Item2, moddable.Item2.type, mod.Item2.type, modItem.Item2.type, modItem.Item2.stack) && modifier.CraftingResultItem > 0)
                    {
                        item2.CloneDefaults(modifier.CraftingResultItem);
                        item2.type = modifier.CraftingResultItem;
                        item2.SetDefaults(modifier.CraftingResultItem);
                        item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked = true;
                    }
                    else
                    {
                        item2.CloneDefaults(moddable.Item2.type);
                        item2.type = moddable.Item2.type;
                        item2.SetDefaults(moddable.Item2.type);
                        item2.GetGlobalItem<ModifiersItem>().Modifier = modifier.ModifierType;
                        item2.prefix = moddable.Item2.prefix;
                        item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked = moddable.Item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked;
                    }
                    resultItem.Item2 = item2;
                }
                if (moddable.Item2 != null && !moddable.Item2.IsAir && mod.Item2 != null && !mod.Item2.IsAir && mod.Item2.ModItem is ModifierContainer cont)
                {
                    Item it = cont.ItemsInside[cont.CurSelection];
                    if (it != null && !it.IsAir && it.ModItem is BaseModifier mod2)
                    {
                        Player local = Main.LocalPlayer;
                        NPC npc = local.GetModPlayer<TranscendencePlayer>().ModifierUINPCPos;

                        Item item2 = new Item();

                        if (IsACraftingRecipe(moddable.Item2, mod.Item2, modItem.Item2, moddable.Item2.type, mod.Item2.type, modItem.Item2.type, modItem.Item2.stack) && mod2.CraftingResultItem > 0)
                        {
                            item2.CloneDefaults(mod2.CraftingResultItem);
                            item2.type = mod2.CraftingResultItem;
                            item2.SetDefaults(mod2.CraftingResultItem);
                            item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked = true;
                        }
                        else
                        {
                            item2.CloneDefaults(moddable.Item2.type);
                            item2.type = moddable.Item2.type;
                            item2.SetDefaults(moddable.Item2.type);
                            item2.GetGlobalItem<ModifiersItem>().Modifier = mod2.ModifierType;
                            item2.prefix = moddable.Item2.prefix;
                            item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked = moddable.Item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked;
                        }
                        resultItem.Item2 = item2;
                    }
                }
            }
        }

        public bool CanApply()
        {
            if (moddable.Item2 != null && mod.Item2 != null && modItem.Item2 != null && !moddable.Item2.IsAir && !mod.Item2.IsAir && !modItem.Item2.IsAir)
            {
                if (mod.Item2.ModItem is BaseModifier bMod && moddable.Item2.GetGlobalItem<ModifiersItem>().Modifier != bMod.ModifierType && modItem.Item2.stack >= bMod.RequiredAmount && modItem.Item2.type == bMod.RequiredItem && bMod.CanBeApplied(moddable.Item2))
                    return true;
                if (mod.Item2.ModItem is ModifierContainer cont)
                {
                    Item it = cont.ItemsInside[cont.CurSelection];
                    if (it != null && !it.IsAir && it.ModItem is BaseModifier mod2 && moddable.Item2.GetGlobalItem<ModifiersItem>().Modifier != mod2.ModifierType && modItem.Item2.stack >= mod2.RequiredAmount && modItem.Item2.type == mod2.RequiredItem && mod2.CanBeApplied(moddable.Item2))
                        return true;
                    return false;
                }
                return false;
            }
            else return false;
        }

        private void Button2_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!ModifierApplierUIDrawing.Visible)
                return;

            Player local = Main.LocalPlayer;
            NPC npc = local.GetModPlayer<TranscendencePlayer>().ModifierUINPCPos;

            if (!local.CanAfford((long)ValueCoins()) || local.CountItem(ModContent.ItemType<CarbonOre>(), ValueCarbon() + 5) < ValueCarbon())
            {
                return;
            }

            if (CanApply() && mod.Item2.ModItem is BaseModifier bMod)
            {
                moddable.Item2.TryGetGlobalItem(out ModifiersItem globalItem);

                if (globalItem.Modifier == bMod.ModifierType)
                    return;

                TryCrafting(moddable.Item2, mod.Item2, modItem.Item2);

                globalItem.Modifier = bMod.ModifierType;
                globalItem.ModifiersUnlocked = true;

                mod.Item2.stack -= 1;
                modItem.Item2.stack -= bMod.RequiredAmount;

                SoundEngine.PlaySound(SoundID.Item37, npc.Center);

                for (int i = 0; i < ValueCarbon(); i++)
                {
                    local.ConsumeItem(ModContent.ItemType<CarbonOre>());
                }

                local.GetModPlayer<TranscendencePlayer>().AmountSpentAtBlacksmith += (int)ValueCoins();
                if (local.GetModPlayer<TranscendencePlayer>().AmountSpentAtBlacksmith > Item.buyPrice(platinum: 3))
                {
                    Item.NewItem(local.GetSource_FromThis(), local.getRect(), ModContent.ItemType<BlacksmithPhoneNumber>());
                    local.GetModPlayer<TranscendencePlayer>().AmountSpentAtBlacksmith = 0;
                }
                local.BuyItem((long)ValueCoins());
            }

            if (CanApply() && mod.Item2.ModItem is ModifierContainer cont)
            {
                Item item = cont.ItemsInside[cont.CurSelection];
                if (item == null || item.IsAir)
                    return;

                moddable.Item2.TryGetGlobalItem(out ModifiersItem globalItem);

                if (item.ModItem is BaseModifier mod2)
                {
                    if (globalItem.Modifier == mod2.ModifierType)
                        return;

                    TryCrafting(moddable.Item2, mod.Item2, modItem.Item2);

                    globalItem.Modifier = mod2.ModifierType;
                    globalItem.ModifiersUnlocked = true;

                    modItem.Item2.stack -= mod2.RequiredAmount;

                    SoundEngine.PlaySound(SoundID.Item37, npc.Center);

                    for (int i = 0; i < ValueCarbon(); i++)
                    {
                        local.ConsumeItem(ModContent.ItemType<CarbonOre>());
                    }

                    local.GetModPlayer<TranscendencePlayer>().AmountSpentAtBlacksmith += (int)ValueCoins();
                    if (local.GetModPlayer<TranscendencePlayer>().AmountSpentAtBlacksmith > Item.buyPrice(platinum: 3))
                    {
                        Item.NewItem(local.GetSource_FromThis(), local.getRect(), ModContent.ItemType<BlacksmithPhoneNumber>());
                        local.GetModPlayer<TranscendencePlayer>().AmountSpentAtBlacksmith = 0;
                    }
                    local.BuyItem((long)ValueCoins());
                }
            }
        }

        public void TryCrafting(Item affectedWeapon, Item upgradeItem, Item ingredientItem)
        {
            AddModifierUpgrades(affectedWeapon, upgradeItem, ingredientItem, ItemID.Muramasa, ModContent.ItemType<MuramasaUpgrade>(), ItemID.Frostbrand, 1);
            AddModifierUpgrades(affectedWeapon, upgradeItem, ingredientItem, ModContent.ItemType<BaseballBat>(), ModContent.ItemType<HardmetalSpike>(), ModContent.ItemType<HardmetalBar>(), 4);
            AddModifierUpgrades(affectedWeapon, upgradeItem, ingredientItem, ItemID.ClockworkAssaultRifle, ModContent.ItemType<ClockworkCog>(), ItemID.Cog, 99);
            AddModifierUpgrades(affectedWeapon, upgradeItem, ingredientItem, ItemID.EyeoftheGolem, ModContent.ItemType<SnowOfInfinity>(), ItemID.SoulofMight, 20);
            AddModifierUpgrades(affectedWeapon, upgradeItem, ingredientItem, ItemID.RocketLauncher, ModContent.ItemType<SpaceScrap>(), ModContent.ItemType<CrystalItem>(), 20);
        }
        public bool IsACraftingRecipe(Item affectedWeapon, Item upgradeItem, Item ing, int affectedWeaponType, int upgradeItemType, int ingType, int ingStack)
        {
            return (affectedWeapon.type == affectedWeaponType && upgradeItem.type == upgradeItemType && ing.type == ingType && ing.stack >= ingStack);
        }
        public void AddModifierUpgrades(Item affectedWeapon, Item upgradeItem, Item ing, int affectedWeaponType, int upgradeItemType, int ingType, int ingStack)
        {
            if (IsACraftingRecipe(affectedWeapon, upgradeItem, ing, affectedWeaponType, upgradeItemType, ingType, ingStack) && upgradeItem.ModItem is BaseModifier modifier)
            {
                affectedWeapon.SetDefaults(modifier.CraftingResultItem);
                affectedWeapon.type = modifier.CraftingResultItem;
                affectedWeapon.GetGlobalItem<ModifiersItem>().ModifiersUnlocked = true;

                if (!Main.dedServ)
                    SoundEngine.PlaySound(SoundID.Item37);
            }
        }

        private void Button_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!ModifierApplierUIDrawing.Visible)
                return;

            if (Main.netMode != NetmodeID.Server)
                SoundEngine.PlaySound(SoundID.MenuClose);

            ModifierApplierUIDrawing.Visible = false;
        }
    }
    public class ModifiableItemSlot : ModifierApplierItemSlot
    {
        
        public override int acceptedItemType => 0;
        public override string icon => "TranscendenceMod/Miscannellous/UI/ModifierUI/ModifiableItemIcon";
        public override string hoverText => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.ModifiableItem");

        public override int xMod => 0;

        public override int yMod => 0;
        public override bool Locked => false;
    }
    public class ModifierItemSlot : ModifierApplierItemSlot
    {

        public override int acceptedItemType => 1;

        public override int xMod => 60;

        public override int yMod => 0;
        public override bool Locked => false;
    }
    public class ModifierResourceSlot : ModifierApplierItemSlot
    {
        public override string icon => "TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierIngredientIcon";
        public override string hoverText => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.ModifierMaterial");
        public override int acceptedItemType => 2;

        public override int xMod => 120;

        public override int yMod => 0;

        public override bool Locked => false;
    }
    public class ModifierResultSlot : ModifierApplierItemSlot
    {
        public override string icon => "TranscendenceMod/Miscannellous/UI/ModifierUI/ModifiableItemIcon";
        public override string hoverText => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.ModifierResult");
        public override int acceptedItemType => 2;

        public override int xMod => 180;

        public override int yMod => 0;

        public override bool Locked => true;
    }
    public abstract class ModifierApplierItemSlot : UIElement
    {
        public Item Item2;
        public Item Item2Temp;
        public int CD;
        public abstract int xMod { get; }
        public abstract int yMod { get; }
        public static int x = Main.screenWidth / 2 - 30;
        public static int y = Main.screenHeight / 2 - 142;
        Rectangle rec = new Rectangle(x, y, 32, 32);
        public bool ShouldBeActive = false;
        public abstract bool Locked { get; }
        /// <summary>
        /// 0 = Modifiable, 1 = Modifier, 2 = Modifier Resource
        /// </summary>
        public abstract int acceptedItemType { get; }
        public virtual string icon => "TranscendenceMod/Miscannellous/UI/ModifierUI/ModifierIcon";
        public virtual string hoverText => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.ModifierItem");
        public bool Hovering => Main.MouseWorld.Distance(new Vector2(Main.screenPosition.X + x + xMod - 16, Main.screenPosition.Y + y + yMod - 16)) < 24;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            x = (int)GetDimensions().X;
            y = (int)GetDimensions().Y - 20;

            if (ShouldBeActive)
            {
                Texture2D bg = TextureAssets.InventoryBack.Value;
                rec = new Rectangle(x + xMod - 32, y + yMod - 32, 42, 42);
                Rectangle recEmpty = new Rectangle(x + xMod - 26, y + yMod - 26, 30, 30);

                ItemSlot.DrawSavings(Main.spriteBatch, 180, Main.instance.invBottom, horizontal: true);

                spriteBatch.Draw(bg, rec, Locked ? Color.White * 0.25f : Color.White);
                if (Item2.IsAir)
                {
                    spriteBatch.Draw(ModContent.Request<Texture2D>(icon).Value, recEmpty, null, Color.White * 0.5f);
                    if (Hovering)
                    {
                        Main.hoverItemName = hoverText;
                        Main.mouseText = true;
                    }
                }
                if (!Item2.IsAir)
                {
                    if (Hovering)
                    {
                        Main.HoverItem = Item2.Clone();
                        Main.hoverItemName = Item2.Clone().Name;
                        Main.mouseText = true;
                    }
                }
                if (Item2 != null && !Item2.IsAir)
                {
                    Vector2 pos = new Vector2(x + xMod - 11, y + yMod - 10);

                    bool shadered = acceptedItemType == 0 && Item2.GetGlobalItem<ModifiersItem>().Modifier != ModifierIDs.None;
                    //Request effect
                    var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/ModifierShader", AssetRequestMode.ImmediateLoad).Value;

                    //Set uImage1 to be Orion Nebula
                    Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/ModifierEffect").Value;
                    Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                    //Make the texture actually scroll with Main.GlobalTimeWrappedHourly
                    eff.Parameters["uImageSize0"].SetValue(new Vector2(500));
                    eff.Parameters["uImageSize1"].SetValue(shaderImage.Size());
                    eff.Parameters["uTime"].SetValue(-Main.GlobalTimeWrappedHourly * 0.5f);
                    eff.Parameters["uSaturation"].SetValue(Main.GlobalTimeWrappedHourly * 0.5f);

                    if (shadered)
                    {
                        spriteBatch.End();
                        spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null);
                    }

                    //spriteBatch.Draw(itemsprite, pos2, frame, Color.White, 0, frame.Size() * 0.5f, scale, SpriteEffects.None, 0);
                    ItemSlot.DrawItemIcon(Item2, ItemSlot.Context.InventoryItem, spriteBatch, pos, 1f, bg.Width * 0.7f, Color.White);
                    if (shadered)
                    {
                        spriteBatch.End();
                        spriteBatch.Begin(default, default, default, default, default, null, Main.UIScaleMatrix);
                    }

                    if (Item2.stack > 1)
                    {
                        string stack = Item2.stack.ToString();
                        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value,
                            stack, new Vector2(x - 20 + xMod, y + yMod), Color.White, 0, Vector2.Zero, Vector2.One * 0.75f);
                    }
                }
            }
        }

        public void TakeItem(Item it)
        {
            it.SetDefaults(Item2.type);
            it.prefix = Item2.prefix;
            it.stack = Item2.stack;

            it.GetGlobalItem<ModifiersItem>().ModifiersUnlocked = Item2.GetGlobalItem<ModifiersItem>().ModifiersUnlocked;
            SoundEngine.PlaySound(SoundID.Grab);

            if (Item2.GetGlobalItem<ModifiersItem>().Modifier > 0)
                it.GetGlobalItem<ModifiersItem>().Modifier = Item2.GetGlobalItem<ModifiersItem>().Modifier;

            if (Item2.ModItem is ModifierContainer cont && it.ModItem is ModifierContainer cont2)
            {
                cont2.ItemsInside = cont.ItemsInside;
                cont2.CurSelection = cont.CurSelection;
            }

            Item2.TurnToAir();
            CD = 0;
        }

        public override void Update(GameTime gameTime)
        {
            ShouldBeActive = true;
            Item item = Main.mouseItem;


            x = (int)GetDimensions().X;
            y = (int)GetDimensions().Y - 20;

            if (Item2 == null)
            {
                Item2 = new Item();
            }

            if (!ShouldBeActive || Locked || !ModifierApplierUIDrawing.Visible)
                return;

            if (!Main.mouseLeft) CD++;

            if (Hovering)
            {
                if (!Item2.IsAir && item.IsAir && (Main.mouseLeft || Main.mouseRight) && CD > 15)
                {
                    if (Main.mouseRight)
                    {
                        for (int i = 0; i < 49; i++)
                        {
                            Item it = Main.LocalPlayer.inventory[i];
                            if (it != null && it.IsAir && Main.LocalPlayer != null)
                            {
                                TakeItem(it);
                                return;
                            }
                        }
                    }
                    else TakeItem(item);
                }
                Main.LocalPlayer.mouseInterface = true;
                bool condition = false;

                if (acceptedItemType == 0)
                    condition = !(item.ModItem is BaseModifier);
                if (acceptedItemType == 1)
                    condition = item.ModItem is BaseModifier || item.type == ModContent.ItemType<ModifierContainer>();
                if (acceptedItemType == 2)
                    condition = !(item.ModItem is BaseModifier);

                if (Item2 != item && item != null && condition && Main.mouseLeft && Item2.IsAir && !item.IsAir && CD > 15)
                {
                    SoundEngine.PlaySound(SoundID.Grab);

                    Item2 = item.Clone();
                    Item2Temp = Item2;
                    
                    item.TurnToAir();

                    Item2.stack = Item2Temp.stack;

                    CD = 0;
                }
            }
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI.ModifierUI
{
    [Autoload(Side = ModSide.Client)]
    public class ModifierApplierUIDrawing : ModSystem
    {
        internal ModifierApplierUIState mauis;
        private UserInterface mauis2;
        public static bool Visible;

        public override void Load()
        {
            mauis = new ModifierApplierUIState();
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
            else Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ModifierUIOnPhone = false;
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Modifier Applier UI", delegate
                {
                    if (Visible) mauis2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}

