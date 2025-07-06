using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;

namespace TranscendenceMod.Tiles.BigTiles.Rubbles
{
    public class SpaceBig02Natural : BaseBigRubble
    {
        public override string spritePath => "TranscendenceMod/Tiles/BigTiles/Rubbles/SpaceBig02";
        public override int dustType => ModContent.DustType<SpaceRockDust>();
        public override Color mapColor => new Color(151, 89, 255);
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            TileObjectData.GetTileData(Type, 0).LavaDeath = false;
        }
    }
    public class SpaceBig02RubbleMaker : BaseBigRubble
    {
        public override string spritePath => "TranscendenceMod/Tiles/BigTiles/Rubbles/SpaceBig02";
        public override int dustType => ModContent.DustType<SpaceRockDust>();
        public override Color mapColor => new Color(151, 89, 255);
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            FlexibleTileWand.RubblePlacementLarge.AddVariations(ModContent.ItemType<SpaceRockItem>(), 0);
            RegisterItemDrop(ModContent.ItemType<SpaceRockItem>());
        }
    }
}
