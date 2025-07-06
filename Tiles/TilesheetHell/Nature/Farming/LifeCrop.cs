using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public class LifeCrop : BaseCrop
    {
        public override int GrowthDivider => 4;
        public override Color mapColor => new Color(219, 157, 64);

        public override void RandomUpdate(int i, int j)
        {
            if (Main.tile[i, j].LiquidAmount >= 25 && Main.tile[i, j].LiquidType == LiquidID.Honey)
                base.RandomUpdate(i, j);
        }

        public override bool CanDrop(int i, int j) => GetAge(i, j) == CropAge.Grown;
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ItemID.LifeFruit, 1);
        }
    }
}
