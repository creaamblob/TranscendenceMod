using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Rarities
{
    public class Brown : ModRarity
    {
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset < 0)
                return ModContent.RarityType<ModdedPurple>();
            if (offset == 2)
                return ModContent.RarityType<MidnightBlue>();
            return Type;
        }
        public override Color RarityColor => new Color(129, 73, 55);
    }
}