using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Rarities
{
    public class SteelGray : ModRarity
    {
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -1)
                return ModContent.RarityType<Brown>();
            if (offset == -2)
                return ModContent.RarityType<ModdedPurple>();

            return Type;
        }
        public override Color RarityColor => new Color(72, 75, 99) * 1.75f;
    }
}