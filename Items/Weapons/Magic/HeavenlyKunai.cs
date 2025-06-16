using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class HeavenlyKunai : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 175;
            Item.knockBack = 2f;

            Item.width = 22;
            Item.height = 22;

            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.shootSpeed = 6.25f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item39;

            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 17, silver: 50);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.shoot = ModContent.ProjectileType<HeavenlyKunaiProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 4; i++)
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(Item.shootSpeed, Item.shootSpeed + 4f) * 0.5f, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}