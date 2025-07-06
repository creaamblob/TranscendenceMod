using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TranscendenceMod.Buffs
{
    public class SeraphTimeStop : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.controlLeft = false;
            player.controlRight = false;
            player.controlJump = false;
            player.controlThrow = false;
            player.controlUp = false;
            player.controlDown = false;

            player.position = player.oldPosition;
            player.direction = player.oldDirection;
            player.velocity = Vector2.Zero;
            player.forcedGravity *= 0;
            player.GetModPlayer<TranscendencePlayer>().CannotUseItems = true;
            player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer = 5;
        }
    }
}
