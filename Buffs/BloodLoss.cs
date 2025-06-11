using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs
{
    public class BloodLoss : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 25;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().BloodDying = true;
            if (player.GetModPlayer<TranscendencePlayer>().PlayerCrimsonNecklaceTimer > 0)
            {
                player.buffTime[buffIndex] += player.GetModPlayer<TranscendencePlayer>().PlayerCrimsonNecklaceTimer;
                player.GetModPlayer<TranscendencePlayer>().PlayerCrimsonNecklaceTimer = 0;

            }
        }
    }
}
