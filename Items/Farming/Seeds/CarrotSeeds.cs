using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming.Seeds
{
    public class CarrotSeeds : BaseSeed
    {
        public override int Tile => ModContent.TileType<CarrotCrop>();
        public override bool allowed(Tile tile2) => tile2.TileType == ModContent.TileType<Soil>() || tile2.TileType == ModContent.TileType<StarSoil>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 25);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
