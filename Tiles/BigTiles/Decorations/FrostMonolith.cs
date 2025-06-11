using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables;

namespace TranscendenceMod.Tiles.BigTiles.Decorations
{
    public class FrostMonolith : BaseMonolith
    {
        public override int DropItem => ModContent.ItemType<SeraphMonolithItem>();

        public override int BreakDust => ModContent.DustType<SnowflakeDust>();

        public override Color mapCol => new Color(61, 118, 172);

        public override string ActiveIcon => "";

        public override string InActiveIcon => "";

        public override bool UseIcon => false;

        public override void Effects(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().ZoneSerpentMonolith = 5;
        }
    }
}
