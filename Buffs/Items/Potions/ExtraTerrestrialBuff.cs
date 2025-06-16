using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.Potions
{
    public class ExtraTerrestrialBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().ExtraTerrestrialEffects = true;
        }
    }
}
