using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public class StarfruitCrop : BaseCrop
    {
        public override int GrowthDivider => Main.dayTime ? 0 : 4;
        public override Color mapColor => new Color(24, 109, 223);

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DustType = DustID.BlueCrystalShard;
        }

        public override bool CanDrop(int i, int j) => GetAge(i, j) == CropAge.Grown;
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Starfruit>(), 1);
        }
    }
}
