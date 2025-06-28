using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Items.Weapons.Ranged.Ammo
{
    public class ShimmerBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 12;
            Item.knockBack = 1;
            Item.ammo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<ShimmerBulletProj>();
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.width = 20;
            Item.height = 20;
            Item.value = 2;
            Item.rare = ItemRarityID.Green;
        }
    }
}
