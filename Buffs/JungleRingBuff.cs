using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Buffs
{
    public class JungleRingBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
            player.lifeRegen += 3;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 25;
            npc.GetGlobalNPC<TranscendenceNPC>().SlowDownType = 1;
        }
    }
}
