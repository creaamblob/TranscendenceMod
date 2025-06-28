using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Modifiers;

namespace TranscendenceMod.Items.Materials
{
    public class BlacksmithPhoneNumber : BaseModifier
    {
        public override bool CanBeApplied(Item item) => item.type == ItemID.REK;
        public override int RequiredItem => ItemID.Wire;
        public override int RequiredAmount => 20;
        public override ModifierIDs ModifierType => ModifierIDs.Blacksmith;
        public override int CraftingResultItem => ModContent.ItemType<BlacksmithPhoneNumber>();
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.BlacksmithPhone");
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 18;
            Item.rare = ItemRarityID.Quest;
        }
    }
}
