using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Modifiers
{
    public class DeathShockCharger : BaseModifier
    {
        public override int RequiredItem => ModContent.ItemType<SoulOfKnight>();
        public override int RequiredAmount => 5;
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.DeathCharger");
        public override ModifierIDs ModifierType => ModifierIDs.DeathCharger;
        public override bool CanBeApplied(Item item) => item.defense > 0 && !item.accessory;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 24;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ModContent.RarityType<Brown>();
        }
    }
}
