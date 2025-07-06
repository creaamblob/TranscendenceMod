using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Skies;

namespace TranscendenceMod.Miscannellous.Biomes
{
    public class Heaven : ModBiome
    {
        public override string BestiaryIcon => "TranscendenceMod/Miscannellous/Assets/HeavenBestiaryIcon";
        public override string MapBackground => "TranscendenceMod/Miscannellous/Assets/MapStar";
        public override string BackgroundPath => "TranscendenceMod/Miscannellous/Assets/MapStar";
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<SpaceSky>();
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<LandsiteWater>();

        public override bool IsBiomeActive(Player player)
        {
            return player.GetModPlayer<TranscendencePlayer>().StarFade > 0;
        }
        public override int BiomeTorchItemType => ItemID.RainbowTorch;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicID.OtherworldlyDungeon;
    }
}