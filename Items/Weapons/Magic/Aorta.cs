using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class Aorta : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 148;
            Item.mana = 10;
            Item.knockBack = 3f;

            Item.width = 26;
            Item.height = 22;

            Item.useTime = 16;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 20);
            Item.useAnimation = 16;
            Item.rare = ModContent.RarityType<Brown>();
            Item.shoot = ModContent.ProjectileType<FriendlyNucleusLaser>();
            Item.shootSpeed = 7.5f;
            Item.UseSound = SoundID.Item103;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            TranscendenceUtils.BasicShotgun(source, position + new Vector2(16 * player.direction, -18), new Vector2(0, -Item.shootSpeed * 10f), type, damage, knockback, 3, 25f, 1f, player.whoAmI, 0f, 1f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ElectricalComponent>())
            .AddIngredient(ModContent.ItemType<SoulOfKnight>(), 4)
            .AddIngredient(ItemID.Ectoplasm, 16)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}