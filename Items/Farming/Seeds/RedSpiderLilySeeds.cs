using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming.Seeds
{
    public class RedSpiderLilySeeds : BaseSeed
    {
        public override int Tile => ModContent.TileType<RedSpiderLily>();
        public override bool allowed(Tile tile2) => tile2.TileType == ModContent.TileType<Soil>() || tile2.TileType == ModContent.TileType<StarSoil>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 25);
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            CreateRecipe(6)
            .AddIngredient(ItemID.DeathweedSeeds, 6)
            .AddIngredient(ItemID.Ectoplasm, 2)
            .AddTile(TileID.Blendomatic)
            .Register();
        }
    }
}
