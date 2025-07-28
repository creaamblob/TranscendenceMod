using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscanellous.UI.Achievements.Tasks;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Modifiers
{
    public abstract class BaseModifier : ModItem
    {
        public virtual ModifierIDs ModifierType => ModifierIDs.None;
        public abstract bool CanBeApplied(Item item);
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

            var modifier = new TooltipLine(Mod, "Modifier Name", Language.GetTextValue($"Mods.TranscendenceMod.Messages.Tooltips.Modifiers.{ModifierType}"));
            tooltips.Insert(2, modifier);

            string plural = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.RequiredPlural", RequiredAmount, RequiredItem);
            string single = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.RequiredSingle", RequiredAmount, RequiredItem);

            var reqs = new TooltipLine(Mod, "Requirements", RequiredAmount > 1 ? plural : single);
            if (!(Item.ModItem is TerrariaDial))
                tooltips.Insert(3, reqs);
        }
    }
    public class ModifierSlotUnlock : ModItem
    {
        
    }
}
