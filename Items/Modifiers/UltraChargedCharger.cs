using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Modifiers
{
    public class UltraChargedCharger : BaseModifier
    {
        public override int RequiredItem => ModContent.ItemType<Lightning>();
        public override int RequiredAmount => 3;
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.Charger");
        public override ModifierIDs ModifierType => ModifierIDs.Charged;
        public override bool CanBeApplied(Item item) => item.damage > 0 && !ItemID.Sets.Yoyo[item.type] && !item.sentry && (item.DamageType != DamageClass.Summon || item.DamageType == DamageClass.SummonMeleeSpeed);

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 24;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Cyan;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<SteelAlloy>(), 4)
            .AddIngredient(ModContent.ItemType<Lightning>(), 6)
            .AddIngredient(ItemID.LogicSensor_Water, 2)
            .AddIngredient(ItemID.Wire, 20)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
