using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Magic;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class WetTome : ModItem
    {
        int proj = ModContent.ProjectileType<CthulhunadoFriendly>();

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 225;
            Item.mana = 12;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 4f;
            Item.crit = 5;

            Item.width = 24;
            Item.height = 32;
            Item.channel = true;
            Item.autoReuse = true;

            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item60;

            Item.value = Item.sellPrice(gold: 17, silver: 50);
            Item.rare = ModContent.RarityType<Brown>();
            Item.shoot = proj;
            Item.shootSpeed = 5f;
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[proj] == 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WaterBolt)
                .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 12)
                .AddTile(ModContent.TileType<Oceation>())
                .Register();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }
    }
}