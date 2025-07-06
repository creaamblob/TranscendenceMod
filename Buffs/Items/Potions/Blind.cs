using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.Potions
{
    public class Blind : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().IsBlind = 5;
        }
    }
}
