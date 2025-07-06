using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class ThrowingPebble : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.knockBack = 1f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.White;
            Item.noMelee = false;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Rock>();
            Item.shootSpeed = 11f;
            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.StoneBlock, 1);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.Register();
        }
    }
}