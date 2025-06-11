using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Rarities
{
    public class ModdedPurple : ModRarity
    {
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset < 0)
                return ItemRarityID.Red;
            if (offset == 2)
                return ModContent.RarityType<Brown>();
            return Type;
        }
        public override Color RarityColor => new Color(180, 40, 255);
    }
}