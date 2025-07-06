using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class BrickFromTheFourthWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = ItemRarityID.Quest;

            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<FourthWallBrick>();
        }
    }
}
