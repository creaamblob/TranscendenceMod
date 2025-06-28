using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables;

namespace TranscendenceMod.Tiles.BigTiles.Decorations
{
    public class SeraphMonolith : BaseMonolith
    {
        public override int DropItem => ModContent.ItemType<SeraphMonolithItem>();

        public override int BreakDust => ModContent.DustType<SpaceRockDust>();

        public override Color mapCol => new Color(122, 20, 177);

        public override string ActiveIcon => "_IconAct";

        public override string InActiveIcon => "_IconIna";

        public override bool UseIcon => true;

        public override void Effects(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().ZoneSeraphMonolith = true;
            player.GetModPlayer<TranscendencePlayer>().SeraphMonolithTimer = 30;
        }
    }
}
