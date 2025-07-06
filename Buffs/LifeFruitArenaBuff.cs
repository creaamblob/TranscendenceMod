using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs
{
    public class LifeFruitArenaBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 += 15;
        }
    }
}
