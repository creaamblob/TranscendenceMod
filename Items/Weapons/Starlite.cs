using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons;

namespace TranscendenceMod.Items.Weapons
{
    public class Starlite : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.knockBack = 2;
            Item.ammo = Item.type;
            Item.shoot = ModContent.ProjectileType<StarGunBullet>();
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.width = 10;
            Item.height = 10;
            Item.value = 2;
            Item.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            CreateRecipe(24)
            .AddIngredient(ItemID.FallenStar, 2)
            .AddTile(TileID.SkyMill)
            .Register();
        }
    }
}
