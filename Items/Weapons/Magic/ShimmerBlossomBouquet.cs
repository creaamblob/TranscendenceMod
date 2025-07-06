using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class ShimmerBlossomBouquet : ModItem
    {
        int proj = ModContent.ProjectileType<ShimmerBlossomProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 75;
            Item.mana = 8;
            Item.knockBack = 3f;

            Item.width = 15;
            Item.height = 27;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 3, silver: 75);
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = proj;
            Item.shootSpeed = 20;
            Item.channel = true;
            Item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ShimmerBlossom>(), 10)
            .AddIngredient(ModContent.ItemType<FlowerEssence>(), 3)
            .AddIngredient(ItemID.SoulofLight, 5)
            .AddIngredient(ItemID.SoulofNight, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}