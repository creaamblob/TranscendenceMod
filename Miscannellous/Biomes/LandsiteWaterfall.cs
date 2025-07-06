using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Biomes
{
    public class LandsiteWaterfall : ModWaterfallStyle
    {
        public override void AddLight(int i, int j)
        {
            Lighting.AddLight(i / 16, j / 16, 0.7f, 0.88f, 0.9f);
        }
    }
}