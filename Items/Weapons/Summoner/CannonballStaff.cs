using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Summoner;

namespace TranscendenceMod.Items.Weapons.Summoner
{
    public class CannonballStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = 3;

            Item.width = 16;
            Item.height = 16;
            Item.noMelee = true;

            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item44;

            Item.mana = 25;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Blue;

            Item.shoot = ModContent.ProjectileType<CannonSentry>();
            Item.shootSpeed = 0f;
            Item.sentry = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.FindSentryRestingSpot(type, out int XPosition, out int YPosition, out int YOffset);
            YOffset -= 60;
            position = new Vector2(XPosition, YPosition + YOffset);

            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}