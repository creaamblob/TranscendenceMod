using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.Potions
{
    public class CrystalRadiationProt : ModBuff
    {
        public override void SetStaticDefaults()
        {

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().CrystalRadiationPill = true;
        }
    }
}
