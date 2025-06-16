using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature;

namespace TranscendenceMod.Miscannellous.Biomes
{
    public class VolcanicBiome : ModBiome
    {
        public override string BestiaryIcon => "TranscendenceMod/Miscannellous/Assets/VolcanoBestiaryIcon";
        public override string MapBackground => $"Terraria/Images/MapBG41";
        public override string BackgroundPath => $"Terraria/Images/MapBG41";
        public override bool IsBiomeActive(Player player)
        {
            return ModContent.GetInstance<VolcanoStonesNearby>().VolcanoStone >= 75;
        }
        public override int BiomeTorchItemType => ItemID.OrangeTorch;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.None;
        public override int Music => -1;
    }
    public class VolcanoStonesNearby : ModSystem
    {
        public int VolcanoStone;
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            VolcanoStone = tileCounts[ModContent.TileType<VolcanicStone>()];
        }
    }
}