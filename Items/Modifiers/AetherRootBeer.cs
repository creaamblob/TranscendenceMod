using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;

namespace TranscendenceMod.Items.Modifiers
{
    public class AetherRootBeer : BaseModifier
    {
        public override int RequiredItem => ModContent.ItemType<Flour>();
        public override int RequiredAmount => 2;
        public override ModifierIDs ModifierType => ModifierIDs.TimedialTerraria;
        public override bool CanBeApplied(Item item) => item.accessory;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 18;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 2, silver: 75);
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<AetherRootItem>(), 8)
            .AddIngredient(ModContent.ItemType<Wheat>(), 4)
            .AddIngredient(ItemID.Ale, 2)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
