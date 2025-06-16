using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.Weapons
{
    public class OvergrownDebuff : ModBuff
    {
        public override string Texture => $"Terraria/Images/Buff_319";
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }
}
