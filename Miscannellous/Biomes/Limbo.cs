using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Skies;
using TranscendenceMod.Tiles.TilesheetHell.Nature;

namespace TranscendenceMod.Miscannellous.Biomes
{
    public class Limbo : ModBiome
    {
        public override string BestiaryIcon => "TranscendenceMod/Miscannellous/Assets/AbyssBestiaryIcon";
        public override string MapBackground => "TranscendenceMod/Miscannellous/Assets/MapStar";
        public override string BackgroundPath => "TranscendenceMod/Miscannellous/Assets/MapStar";
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<NullSky>();
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<LandsiteWater>();

        public override bool IsBiomeActive(Player player)
        {
            return ModContent.GetInstance<SpaceBlocksNearby>().SpaceBlocks >= 1000;
        }
        public override int BiomeTorchItemType => ItemID.DemonTorch;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicID.TheTowers;
    }
    public class SpaceBlocksNearby : ModSystem
    {
        public int SpaceBlocks;
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            SpaceBlocks = tileCounts[ModContent.TileType<VoidTile>()];
        }
    }
}