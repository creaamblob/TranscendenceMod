using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs
{
    public class PrismaticBurn : ModBuff
    {
        public int npcDoT = 35;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= npcDoT;
            if (npc.oiled) npcDoT = 60;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().EolBurn = true;
        }
    }
}
