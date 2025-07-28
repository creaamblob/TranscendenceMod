using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Rarities
{
    public class BloodRed : ModRarity
    {
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset < 0)
                return ModContent.RarityType<CosmicRarity>();
            return Type;
        }
        public override Color RarityColor => new Color(255, 50, 0);
    }
}