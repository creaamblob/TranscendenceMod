using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Rarities
{
    public class MidnightBlue : ModRarity
    {
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -2)
                return ModContent.RarityType<ModdedPurple>();
            if (offset == -1)
                return ModContent.RarityType<Brown>();
            return Type;
        }
        public override Color RarityColor => new Color(0, 100, 200);
    }
}