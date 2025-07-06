using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Miscannellous.Biomes
{
    public class LandsiteWater : ModWaterStyle
    {
        public override int ChooseWaterfallStyle() => ModContent.GetInstance<LandsiteWaterfall>().Slot;

        public override int GetDropletGore() => 710;
        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 0.75f;
            g = 1;
            b = 1;
        }
        public override Color BiomeHairColor() => new Color(185, 225, 227);

        public override int GetSplashDust() => ModContent.DustType<LandsiteDroplet>();
    }
}