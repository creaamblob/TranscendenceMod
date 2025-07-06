using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.TilesheetHell.Bricks;

namespace TranscendenceMod.Items.Consumables.Placeables.Sunken
{
    public class SunkenPinkBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 15;
            Item.rare = ItemRarityID.White;
            Item.maxStack = 9999;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<SunkenBrickPink>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PinkBrick)
                .AddIngredient(ItemID.Coral)
                .AddCondition(Condition.NearWater)
                .Register();
        }
    }
}
