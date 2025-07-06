using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming
{
    public class IronHoe : BaseHoe
    {
        public override ushort soil => (ushort)ModContent.TileType<Soil>();

        public override int range => 3;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;

            Item.width = 18;
            Item.height = 18;

            Item.useAnimation = 20;
            Item.useTime = 20;

            Item.value = Item.buyPrice(gold: 5);
        }
    }
}
