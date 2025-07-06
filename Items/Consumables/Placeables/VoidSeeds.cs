using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Nature;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class VoidSeeds : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;

            Item.rare = ItemRarityID.Quest;
            Item.maxStack = 9999;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(silver: 5);
        }
        public static bool VoidConsumable(Tile tile)
        {
            if (tile.TileType == TileID.ClosedDoor && tile.TileFrameY == 594)
                return false;

            return tile.TileType != ModContent.TileType<VoidTile>() && tile.TileType != TileID.LihzahrdBrick && tile.TileType != TileID.Cobweb;
        }
        public override bool? UseItem(Player player)
        {
            Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
            if (VoidConsumable(tile) && tile.HasTile && Main.tileSolid[tile.TileType] && (player.Distance(Main.MouseWorld) < ((7 + Item.tileBoost) * 16)))
            {
                Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)ModContent.TileType<VoidTile>();
                SoundEngine.PlaySound(SoundID.Tink);
                return true;
            }
            else return false;
        }
    }
}
