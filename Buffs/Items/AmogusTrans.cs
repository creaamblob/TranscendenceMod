using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items
{
    public class AmogusTrans : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
