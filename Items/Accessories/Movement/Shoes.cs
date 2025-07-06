using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Movement
{
    public class Shoes : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Blue;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().WearingEarlygameShoes = 2;
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().WearingEarlygameShoes = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Leather, 8)
                .AddRecipeGroup(RecipeGroupID.IronBar, 4)
                .AddIngredient(ItemID.SwiftnessPotion, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
