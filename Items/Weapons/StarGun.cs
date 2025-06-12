using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons;

namespace TranscendenceMod.Items.Weapons
{
    public class StarGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
            Item.damage = 15;
            Item.mana = 5;
            Item.useAmmo = AmmoID.Bullet;
            Item.knockBack = 2;

            Item.width = 28;
            Item.height = 12;

            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.useAmmo = ModContent.ItemType<Starlite>();

            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20;
            Item.UseSound = SoundID.Item158;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.statMana > Item.mana && player.HasAmmo(Item))
            {
                return base.CanUseItem(player);
            }
            else return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<StarGunBullet>();
            Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(Item, Item.ammo), position, velocity, type, damage, knockback, player.whoAmI, 0, -1);
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 0f);
        }
    }
}