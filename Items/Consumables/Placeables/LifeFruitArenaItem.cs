using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class LifeFruitArenaItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.HeartLantern);
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Yellow;
            Item.DefaultToPlaceableTile(ModContent.TileType<LifeFruitArenaTile>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LifeFruit, 2)
            .AddIngredient(ItemID.RichMahogany, 5)
            .Register();
        }
    }
}
