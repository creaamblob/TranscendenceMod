using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Weapons.Reforges
{
    public class Crystallized : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Magic;
        public override float RollChance(Item item)
        {
            return 1.25f;
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1f - 0.15f;
            manaMult *= 1f - 0.4f;
        }
        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1f * 0.09f;
        }
    }
}