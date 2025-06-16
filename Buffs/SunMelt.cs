using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs
{
    public class SunMelt : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 75;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().SunMelt = true;
        }
    }
}
