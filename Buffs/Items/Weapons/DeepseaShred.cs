using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Buffs.Items.Weapons
{
    public class DeepseaShred : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<TranscendenceNPC>().DeepseaDebuff = true;
            npc.lifeRegen -= 310;
        }
    }
}
