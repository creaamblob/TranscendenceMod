using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class SpazzingEye : BaseModifier
    {
        public override int RequiredItem => ItemID.SoulofSight;
        public override int RequiredAmount => 5;
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.Spazzing");
        public override ModifierIDs ModifierType => ModifierIDs.Spazzy;
        public override bool CanBeApplied(Item item) => item.headSlot != -1;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.buyPrice(gold: 7, silver: 50);
            Item.rare = ItemRarityID.LightRed;
        }
    }
}
