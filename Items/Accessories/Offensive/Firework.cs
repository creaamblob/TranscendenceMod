using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Offensive
{
    public class Firework : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 24;

            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().RocketAcc = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Dynamite, 3)
            .AddIngredient(ItemID.ExplosivePowder, 17)
            .AddIngredient(ItemID.BambooBlock, 5)
            .AddIngredient(ItemID.RedHusk)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}
