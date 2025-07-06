using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Modifiers
{
    public abstract class BaseModifier : ModItem
    {
        /// <summary>
        /// 0 = None, 1 = Luminous, 2 = Spazzy, 3 = Danger Detecting, 4 = Sucking, 5 = Mechanical, 6 = Giant-Slayer,
        /// Charger = 7, 8 = Scroll, 9 = Magnet, 10 = Pearl, 11 = Long Pick Head, 12 = Giant Handle
        /// </summary>
        public virtual ModifierIDs ModifierType => ModifierIDs.None;
        public abstract bool CanBeApplied(Item item);
        public virtual string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.Luminous");
        public virtual int RequiredItem => ItemID.IronPickaxe;
        public virtual int RequiredAmount => 1;
        public virtual int CraftingResultItem => -1;
        public override void SetDefaults()
        {
            Item.GetGlobalItem<ModifiersItem>().Modifier = ModifierType;
            Item.maxStack = Item.CommonMaxStack;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var modifierLore = new TooltipLine(Mod, "Modifier Lore", Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.ModifierLore"));
            tooltips.Insert(1, modifierLore);

            var modifier = new TooltipLine(Mod, "Modifier Name", TooltipPath);
            tooltips.Insert(2, modifier);

            string plural = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.RequiredPlural", RequiredAmount, RequiredItem);
            string single = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.RequiredSingle", RequiredAmount, RequiredItem);

            var reqs = new TooltipLine(Mod, "Requirements", RequiredAmount > 1 ? plural : single);
            if (!(Item.ModItem is EarthDial))
                tooltips.Insert(3, reqs);
        }
    }
    public class ModifierSlotUnlock : ModItem
    {
        
    }
}
