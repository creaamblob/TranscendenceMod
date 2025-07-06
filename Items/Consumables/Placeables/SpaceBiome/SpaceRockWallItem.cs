using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Walls.Natural;

namespace TranscendenceMod.Items.Consumables.Placeables.SpaceBiome
{
    public class SpaceRockWallItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<SpaceRockWall>());
        }
        public override void AddRecipes()
        {
            CreateRecipe(4)
            .AddIngredient(ModContent.ItemType<SpaceRockItem>())
            .AddTile(ModContent.TileType<ShimmerAltar>())
            .Register();
        }
    }
}
