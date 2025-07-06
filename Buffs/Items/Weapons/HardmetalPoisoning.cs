using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.Weapons
{
    public class HardmetalPoisoning : ModBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 3;
        }
    }
}
