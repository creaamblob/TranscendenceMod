using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Items.Farming;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public class TomatoCrop : BaseCrop
    {
        public override int GrowthDivider => 3;
        public override Color mapColor => new Color(179, 20, 43);

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            Item item = null;
            if (GetAge(i, j) == CropAge.Grown)
                item = new Item(ModContent.ItemType<Tomato>(), 1);

            yield return item;
        }
    }
}
