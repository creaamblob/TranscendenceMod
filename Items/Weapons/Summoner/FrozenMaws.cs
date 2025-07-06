using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Magic;
using TranscendenceMod.Projectiles.Weapons.Summoner;

namespace TranscendenceMod.Items.Weapons.Summoner
{
    public class FrozenMaws : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            Item.staff[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 85;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = 1f;

            Item.width = 14;
            Item.height = 20;
            Item.noMelee = true;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item44;

            Item.mana = 15;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<ModdedPurple>();

            Item.shoot = ModContent.ProjectileType<FrozenMawsProj>();
            Item.shootSpeed = 3f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float rot = Main.rand.NextFloat(MathHelper.TwoPi);
            for (int i = 0; i < 5; i++)
                Projectile.NewProjectile(source, Main.MouseWorld, velocity.RotatedBy(MathHelper.TwoPi * i / 5f + rot), type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}