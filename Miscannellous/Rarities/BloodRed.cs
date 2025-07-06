using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Rarities
{
    public class BloodRed : ModRarity
    {
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset < 0)
                return ModContent.RarityType<MidnightBlue>();
            return ModContent.RarityType<DarkPink>();
        }
        public override Color RarityColor => new Color(150, 20, 30);
    }
}