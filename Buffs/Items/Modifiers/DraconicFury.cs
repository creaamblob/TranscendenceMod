using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.Modifiers
{
    public class DraconicFury : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().DraconicFury = true;
        }
    }
}
