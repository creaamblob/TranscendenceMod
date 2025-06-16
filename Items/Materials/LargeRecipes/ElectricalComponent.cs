using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Materials.LargeRecipes
{
    public class ElectricalComponent : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.buyPrice(gold: 17, silver: 50);
            Item.rare = ModContent.RarityType<Brown>();
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<SoulOfKnight>(), 6)
            .AddRecipeGroup(nameof(ItemID.TitaniumBar), 15)
            .AddIngredient(ModContent.ItemType<SteelAlloy>(), 15)
            .AddIngredient(ItemID.Wire, 50)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddIngredient(ItemID.SoulofMight, 10)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddIngredient(ModContent.ItemType<Lightning>(), 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
