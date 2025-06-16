using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.Modifiers
{
    public class DraconicFuryCD : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
    }
}
