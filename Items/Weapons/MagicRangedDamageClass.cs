using Terraria.ModLoader;

namespace TranscendenceMod.Items.Weapons
{
    public class MagicRangedDamageClass : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Ranged)
            {
                new StatInheritanceData(
                damageInheritance: 0.75f,
                critChanceInheritance: 1f,
                attackSpeedInheritance: 1f,
                armorPenInheritance: 1f,
                knockbackInheritance: 1);
            }
            if (damageClass == Magic)
            {
                new StatInheritanceData(
                damageInheritance: 0.75f,
                critChanceInheritance: 1f,
                attackSpeedInheritance: 1f,
                armorPenInheritance: 1f,
                knockbackInheritance: 1);
            }

            return new StatInheritanceData(
				damageInheritance: 0f,
				critChanceInheritance: 0f,
				attackSpeedInheritance: 0f,
				armorPenInheritance: 0f,
				knockbackInheritance: 0f
			);
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Ranged || damageClass == Magic)
                return true;

            return false;
        }
    }
}
