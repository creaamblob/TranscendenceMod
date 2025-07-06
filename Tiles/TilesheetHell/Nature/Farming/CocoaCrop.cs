using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Farming;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public class CocoaCrop : BaseCrop
    {
        public override int GrowthDivider => 4;
        public override Color mapColor => new Color(136, 67, 32);

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            HitSound = SoundID.Dig;
        }

        public override bool CanDrop(int i, int j) => GetAge(i, j) >= CropAge.Leaves;
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<CocoaBean>(), Main.rand.Next(1, GetAge(i, j) == CropAge.Grown ? 5 : 3));
        }
    }
}
