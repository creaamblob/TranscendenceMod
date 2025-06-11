using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Melee;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class DysphoricThrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.Yoyo[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 265;
            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.width = 16;
            Item.height = 16;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;

            Item.knockBack = 1.75f;
            Item.value = Item.buyPrice(gold: 25);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<DysphoricYoyoProj>();
            Item.crit = 12;
            Item.shootSpeed = 12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<SpaceRockItem>(), 14)
            .AddIngredient(ModContent.ItemType<CrystalItem>(), 6)
            .AddIngredient(ItemID.MartianConduitPlating, 12)
            .AddIngredient(ItemID.BeetleHusk, 2)
            .AddTile(ModContent.TileType<ShimmerAltar>())
            .Register();
        }
    }
}