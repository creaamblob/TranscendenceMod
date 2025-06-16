using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs
{
    public class SpaceDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().SpaceBossDot = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 12;
        }
    }
}
