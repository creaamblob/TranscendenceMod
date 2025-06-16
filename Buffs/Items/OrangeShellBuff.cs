using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Shields;

namespace TranscendenceMod.Buffs.Items
{
    public class OrangeShellBuff : ModBuff
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
            player.mount.SetMount(ModContent.MountType<OrangeShellMount>(), player);
            player.buffTime[buffIndex] = 15;
        }
    }
}
