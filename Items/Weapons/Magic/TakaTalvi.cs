using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class TakaTalvi : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 102;
            Item.mana = 75;
            Item.knockBack = 1;

            Item.width = 17;
            Item.height = 31;

            Item.useTime = 4;
            Item.useAnimation = 20;
            Item.reuseDelay = 45;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 30);
            Item.rare = ModContent.RarityType<Brown>();
            Item.shoot = ModContent.ProjectileType<Lumihiutale>();
            Item.shootSpeed = 15;
            Item.UseSound = SoundID.Item109;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 55;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 8; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.075f), type, damage, knockback, player.whoAmI);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Lumimyrsky>())
            .AddIngredient(ItemID.SpectreStaff)
            .AddIngredient(ModContent.ItemType<AtmospheragonScale>(), 4)
            .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 8)
            .AddIngredient(ItemID.FrostCore, 2)
            .AddIngredient(ItemID.SnowBlock, 250)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}