using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.TilesheetHell.Nature;

namespace TranscendenceMod.Items.Consumables.Placeables.SpaceBiome
{
    public class CrystalItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 15;
            Item.value = Item.sellPrice(silver: 25);
            Item.rare = ItemRarityID.LightPurple;
            Item.maxStack = 9999;

            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<SpaceCrystal>();
        }
    }
}
