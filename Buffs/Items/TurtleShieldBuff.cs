using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Shields;

namespace TranscendenceMod.Buffs.Items
{
    public class TurtleShieldBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().InsideShell = 5;
            player.mount.SetMount(ModContent.MountType<JungleShell1>(), player);
            player.buffTime[buffIndex] = 15;
        }
    }
}
