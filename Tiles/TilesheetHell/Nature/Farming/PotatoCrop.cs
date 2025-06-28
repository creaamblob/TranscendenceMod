using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Items.Farming;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public class PotatoCrop : BaseCrop
    {
        public override int GrowthDivider => 1;
        public override Color mapColor => new Color(142, 99, 55);

        public override bool CanDrop(int i, int j) => GetAge(i, j) >= CropAge.Leaves;
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<Potato>(), Main.rand.Next(1, (int)GetAge(i, j)));
        }
    }
}
