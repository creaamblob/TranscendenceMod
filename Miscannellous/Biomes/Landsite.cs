using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Biomes
{
    public class Landsite : ModBiome
    {
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<LandsiteWater>();
        public override bool IsBiomeActive(Player player)
        {
            return player.GetModPlayer<TranscendencePlayer>().ZoneLandSite;
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public string time = Main.dayTime ? "Day" : "Night";
        public override int Music => Main.dayTime ? MusicLoader.GetMusicSlot("TranscendenceMod/Miscannellous/Assets/Sounds/Music/CosmicFieldDay") : MusicLoader.GetMusicSlot("TranscendenceMod/Miscannellous/Assets/Sounds/Music/CosmicFieldNight");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
    }
}