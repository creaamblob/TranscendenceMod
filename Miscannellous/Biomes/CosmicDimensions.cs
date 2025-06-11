using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Biomes
{
    public class CosmicDimensions : ModBiome
    {
        public override string BestiaryIcon => "TranscendenceMod/Miscannellous/Assets/AscendantBestiaryIcon";
        public override string MapBackground => "TranscendenceMod/Miscannellous/Assets/MapStar";
        public override string BackgroundPath => "TranscendenceMod/Miscannellous/Assets/MapStar";
        public override bool IsBiomeActive(Player player) => player.GetModPlayer<TranscendencePlayer>().ZoneStar;
        public override SceneEffectPriority Priority => SceneEffectPriority.None;
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<LandsiteWater>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
}