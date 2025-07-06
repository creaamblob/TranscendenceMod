using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items
{
    public class OverloadedHeartCD : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }
}
