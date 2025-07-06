using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class GangstaShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 42;
            Item.knockBack = 3f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 14f;

            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.autoReuse = true;

            Item.UseSound = SoundID.Item36;
            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.width = 24;
            Item.height = 14;
            Item.noMelee = true;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 5, silver: 25);

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < Main.rand.Next(2, 5); i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.8f, 1.2f), type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.8f, 1.2f), ProjectileID.SnowBallFriendly, damage / 3, knockback, player.whoAmI);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }
    }
}