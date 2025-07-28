using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming
{
    public class AngelHornScythe : BaseHoe
    {
        public override ushort soil => (ushort)ModContent.TileType<StarSoil>();

        public override int range => 7;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ModContent.RarityType<CosmicRarity>();

            Item.width = 26;
            Item.height = 26;

            Item.useAnimation = 12;
            Item.useTime = 12;

            Item.value = Item.buyPrice(gold: 50);
        }
    }
}
