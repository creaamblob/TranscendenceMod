using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.InfectionAccessories
{
    public class EolTrans : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
