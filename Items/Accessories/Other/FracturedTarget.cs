using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Other
{
    public class FracturedTarget : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 30;
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Red;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ReconScope)
            .AddIngredient(ItemID.LivingFireBlock, 100)
            .AddIngredient(ItemID.LunarTabletFragment, 20)
            .AddIngredient(ItemID.DarkShard, 5)
            .AddTile(TileID.LihzahrdFurnace)
            .Register();
        }
    }
}
