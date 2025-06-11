using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class MosquitoSword : ModItem
    {
        int projectile = ModContent.ProjectileType<MosquitoBiteGas>();
        public override void SetStaticDefaults() => CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        public override void SetDefaults()
        {
            Item.damage = 139;
            Item.DamageType = DamageClass.Melee;
            Item.width = 25;
            Item.height = 21;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.7174804f;
            //Item.shoot = 1;
            //Item.shootSpeed = 3;
            Item.value = Item.buyPrice(gold: 11, copper: 28);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
        }
        /*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            TranscendenceUtils.ProjectileShotgun(velocity, position, source, projectile, (int)(damage * 0.7f), knockback, 0.4f, 6);
            return false;
        }*/
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit = 10;
        public override bool? CanHitNPC(Player player, NPC target)
        {
            if (target.townNPC) return true;
            return base.CanHitNPC(player, target);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int projAmount = 4;
            target.AddBuff(BuffID.Venom, 150);
            //SoundEngine.PlaySound(SoundID.Item16, target.Center);
            if (hit.Crit)
            {
                target.AddBuff(ModContent.BuffType<JungleRingBuff>(), 300);
                player.AddBuff(ModContent.BuffType<JungleRingBuff>(), 420);
                player.Heal(damageDone / 10);
            }
            for (int i = 0; i < projAmount; i++)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Main.rand.NextVector2CircularEdge(1f, 0.5f),
                    projectile, (int)(damageDone * 0.33f), 2, player.whoAmI, 1);
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ItemID.Flymeal);
            recipe1.AddIngredient(ItemID.ChlorophyteClaymore);
            recipe1.AddIngredient(ModContent.ItemType<MosquitoVenom>(), 4);
            recipe1.AddIngredient(ModContent.ItemType<LivingOrganicMatter>());
            recipe1.AddTile(TileID.LunarCraftingStation);
            recipe1.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.Flymeal);
            recipe2.AddIngredient(ItemID.ChlorophyteSaber);
            recipe2.AddIngredient(ModContent.ItemType<MosquitoVenom>(), 4);
            recipe2.AddIngredient(ModContent.ItemType<LivingOrganicMatter>());
            recipe2.AddTile(TileID.LunarCraftingStation);
            recipe2.Register();
        }
    }
}