using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Modifiers
{
    public class BlackholeMod : BaseModifier
    {
        public override int RequiredItem => ItemID.FragmentVortex;
        public override int RequiredAmount => 8;
        public override ModifierIDs ModifierType => ModifierIDs.Blackhole;
        public override bool CanBeApplied(Item item) => item.accessory;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ModContent.RarityType<ModdedPurple>();
        }
    }
}
