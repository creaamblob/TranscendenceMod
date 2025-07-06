using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs
{
    public class BlackHoleDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().SuckedIn = true;
        }
    }
}
