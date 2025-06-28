using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs
{
    public class ShieldBreak : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().BrokenShield = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense -= 5;
        }
    }
}
