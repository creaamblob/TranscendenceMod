using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs
{
    public class Noheal : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().NoHealing = true;
        }
    }
}
