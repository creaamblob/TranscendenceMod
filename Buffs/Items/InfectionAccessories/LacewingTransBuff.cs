using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.InfectionAccessories
{
    public class LacewingTransBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);

            if (player.GetModPlayer<TranscendencePlayer>().LacewingTrans)
                player.buffTime[buffIndex] = 5;
        }
    }
}
