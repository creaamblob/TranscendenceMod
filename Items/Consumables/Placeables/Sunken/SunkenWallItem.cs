using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Walls.Natural;

namespace TranscendenceMod.Items.Consumables.Placeables.Sunken
{
    public class SunkenWallItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<SunkenCataWall>());
        }
        public override void AddRecipes()
        {
            Recipe blue = CreateRecipe();
            blue.AddIngredient(ItemID.BlueBrickWall);
            blue.AddIngredient(ItemID.Coral);
            blue.AddCondition(Condition.NearWater);

            Recipe green = CreateRecipe();
            green.AddIngredient(ItemID.GreenBrickWall);
            green.AddIngredient(ItemID.Coral);
            green.AddCondition(Condition.NearWater);

            Recipe pink = CreateRecipe();
            pink.AddIngredient(ItemID.PinkBrickWall);
            pink.AddIngredient(ItemID.Coral);
            pink.AddCondition(Condition.NearWater);
        }
    }
}
