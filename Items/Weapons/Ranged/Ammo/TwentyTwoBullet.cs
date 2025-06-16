using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Items.Weapons.Ranged.Ammo
{
    public class TwentyTwoBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 222;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;

            Item.damage = 44;
            Item.knockBack = 2.2f;

            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<TwentyTwoBulletProj>();

            Item.consumable = true;
            Item.maxStack = 9999;

            Item.width = 16;
            Item.height = 24;

            Item.value = Item.buyPrice(silver: 15);
            Item.rare = ModContent.RarityType<Brown>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(222)
            .AddIngredient(ItemID.HighVelocityBullet, 222)
            .AddIngredient(ModContent.ItemType<SoulOfKnight>())
            .AddIngredient(ItemID.BeeWax, 2)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
