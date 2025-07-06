using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming.Seeds
{
    public class WheatSeed : BaseSeed
    {
        public override int Tile => ModContent.TileType<PotatoCrop>();
        public override bool allowed(Tile tile2) => tile2.TileType == ModContent.TileType<Soil>() || tile2.TileType == ModContent.TileType<StarSoil>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.White;
        }
    }
}
