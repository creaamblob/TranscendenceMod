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
    public class StarLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
            Item.damage = 50;
            Item.mana = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.knockBack = 1;

            Item.width = 34;
            Item.height = 17;

            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.useAmmo = ModContent.ItemType<Starlite>();

            Item.value = Item.sellPrice(gold: 17, silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10;
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
            type = ModContent.ProjectileType<StarLaser>();
            position = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 500;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, -4f);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StarGun>())
            .AddRecipeGroup(nameof(ItemID.TitaniumBar), 15)
            .AddIngredient(ItemID.SoulofSight, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}