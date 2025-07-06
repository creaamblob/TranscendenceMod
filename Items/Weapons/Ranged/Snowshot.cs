using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class Snowshot : ModItem
    {
        public float rotation = -MathHelper.PiOver4;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 182;
            Item.knockBack = 1.5f;
            Item.crit = 10;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 28f;

            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;

            Item.width = 20;
            Item.height = 8;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<ModdedPurple>();

        }
        public override Vector2? HoldoutOffset()
        {
            return Vector2.Zero;//new Vector2(7f, 2f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[p].GetGlobalProjectile<TranscendenceProjectiles>().SnowArrow = true;

            rotation += MathHelper.PiOver4;
            if (rotation > MathHelper.PiOver2)
                rotation = -MathHelper.PiOver4;
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedBy(rotation / 4f);
        }
    }
}