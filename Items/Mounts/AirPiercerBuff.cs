using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Mounts
{
    public class AirPiercerBuff : ModBuff
    {
        public float Rotater;

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<AirPiercerBroom>(), player);
            player.buffTime[buffIndex] = 15;
        }
    }
}
