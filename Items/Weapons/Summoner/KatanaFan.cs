using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Weapons;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Summoner;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Weapons.Summoner
{
    public class KatanaFan : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 118;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = 2f;

            Item.width = 20;
            Item.height = 20;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.mana = 15;
            Item.value = Item.sellPrice(gold: 17, silver: 50);
            Item.rare = ModContent.RarityType<Brown>();

            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MuramasaSummon>();
            Item.shootSpeed = 0f;
            Item.buffType = ModContent.BuffType<MuramasaMinionBuff>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<MuramasaMinionBuff>(), 60);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Smolstar)
                .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 12)
                .AddIngredient(ItemID.GoldBar, 8)
                .AddIngredient(ItemID.Bone, 99)
                .AddTile(ModContent.TileType<Oceation>())
                .Register();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => position = Main.MouseWorld;
    }
}