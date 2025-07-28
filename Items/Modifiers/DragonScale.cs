using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class DragonScale : BaseModifier
    {
        public override int RequiredItem => ItemID.DefenderMedal;
        public override int RequiredAmount => 5;
        public override ModifierIDs ModifierType => ModifierIDs.Draconic;
        public override bool CanBeApplied(Item item) => item.defense > 0 && !item.accessory;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 17, silver: 50);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
