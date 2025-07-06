using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Buffs
{
    public class EarthernFortification : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<TranscendenceNPC>().EarthernDebuff = true;
            npc.GetGlobalNPC<TranscendenceNPC>().SlowDownType = 3;
        }
    }
}
