using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs
{
    public class FrostBite : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 20;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().FrostBite = true;
        }
    }
}
