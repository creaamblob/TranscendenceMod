using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.TerrestrialSecond;

namespace TranscendenceMod.Items.Consumables.Placeables.TerrestrialItems
{
    public class ExtraTerrestrialPlatformItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 200;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 12;
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.maxStack = 9999;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<ExtraTerrestrialPlatform>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ModContent.ItemType<SpaceRockItem>())
                .AddTile(ModContent.TileType<ShimmerAltar>())
                .Register();

            Recipe recipe = Recipe.Create(ModContent.ItemType<SpaceRockItem>());
            recipe.AddIngredient(Type, 2);
            recipe.AddTile(ModContent.TileType<ShimmerAltar>());
            recipe.Register();
        }
    }
}
