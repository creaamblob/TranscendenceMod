using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Melee;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class DeepwaterRippers : ModItem
    {
        public int proj = ModContent.ProjectileType<DeepwaterSlash>();
        public bool Charged;
        public int Charge;
        public int ChargeLossTimer;
        public int ChargeCD = 450;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 44;

            Item.knockBack = 0.2f;
            Item.width = 16;
            Item.height = 28;

            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.crit = 18;

            Item.shoot = proj;
            Item.shootSpeed = 18;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FetidBaghnakhs)
            .AddIngredient(ItemID.SharkFin, 10)
            .AddIngredient(ItemID.BloodWater, 7)
            .AddIngredient(ModContent.ItemType<Lightning>(), 3)
            .AddTile(ModContent.TileType<Oceation>())
            .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vel = velocity.RotatedByRandom(MathHelper.ToRadians(25));
            Projectile.NewProjectile(source, position, vel, type, damage, knockback, player.whoAmI, 0, 0, -1);
            Projectile.NewProjectile(source, position, vel, type, damage, knockback, player.whoAmI, 0, 0, 1);
            return false;
        }
        public override bool MeleePrefix() => true;
    }
}