using Microsoft.Xna.Framework;
using System;
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
    public class DreamSeal : ModItem
    {
        int proj = ModContent.ProjectileType<DreamSealProj>();
        int aura = ModContent.ProjectileType<DreamSealOrb>();
        public int Combo;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 225;
            Item.mana = 10;
            Item.knockBack = 2;
            Item.crit = 10;

            Item.width = 18;
            Item.height = 24;

            Item.useTime = 5;
            Item.useAnimation = 5;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.buyPrice(gold: 35);
            Item.rare = ModContent.RarityType<ModdedPurple>();

            Item.shoot = proj;
            Item.shootSpeed = 18f;

            Item.channel = true;
            Item.UseSound = SoundID.Item1;
        }
        public override bool AltFunctionUse(Player player) => player.ownedProjectileCounts[aura] == 0 ? true : false;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Combo++;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p != null && p.active && p.type == aura && p.owner == player.whoAmI)
                {
                    position = p.Center;
                }
            }

            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[aura] == 0)
                Projectile.NewProjectile(source, player.Center, Vector2.Zero, aura, 50, 3, player.whoAmI);
            else
                Projectile.NewProjectile(source, position, velocity.RotatedBy(Math.Sin(TranscendenceWorld.UniversalRotation * 12f) * 0.175f), proj, damage, knockback, player.whoAmI, 0, 0, Combo % 2 == 0 ? 1 : 0);

            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<EasternTalismans>())
             .AddIngredient(ItemID.FragmentNebula, 20)
             .AddIngredient(ItemID.BambooBlock, 15)
             .AddIngredient(ItemID.LunarOre, 12)
             .AddTile(TileID.LunarCraftingStation)
             .Register();
        }
    }
}