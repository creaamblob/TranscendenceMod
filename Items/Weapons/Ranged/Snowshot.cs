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
            Item.damage = 175;
            Item.knockBack = 1.75f;
            Item.crit = 15;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 28f;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;

            Item.width = 8;
            Item.height = 28;
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

            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }
    }
}