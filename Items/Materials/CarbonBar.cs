using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles;

namespace TranscendenceMod.Items.Materials
{
    public class CarbonBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 10;
            Item.value = Item.buyPrice(silver: 15);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;

            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<CarbonBarTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<CarbonOre>(), 3)
            .AddTile(TileID.Furnaces)
            .Register();
        }
    }
}
