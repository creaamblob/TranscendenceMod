using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class SkullMod : BaseModifier
    {
        public override int RequiredItem => ItemID.PoisonedKnife;
        public override int RequiredAmount => 25;
        public override ModifierIDs ModifierType => ModifierIDs.DangerDetecting;
        public override bool CanBeApplied(Item item) => item.headSlot != -1;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 12;
            Item.height = 12;
            Item.value = Item.buyPrice(gold: 1, silver: 25);
            Item.rare = ItemRarityID.Green;
        }
    }
}
