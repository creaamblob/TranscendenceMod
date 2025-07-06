using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class EasternTalismans : ModItem
    {
        int proj = ModContent.ProjectileType<EasternTalismanProj>();
        public int Combo;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 55;
            Item.mana = 10;
            Item.knockBack = 2;
            Item.crit = 5;

            Item.width = 18;
            Item.height = 24;

            Item.useTime = 24;
            Item.useAnimation = 24;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 7, silver: 50);
            Item.rare = ItemRarityID.Yellow;

            Item.shoot = proj;
            Item.shootSpeed = 20;

            Item.channel = true;
            Item.UseSound = SoundID.Item1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool white = Combo % 2 == 0;

            if (Combo < 6)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(1.25f, 1.35f), type, damage, knockback, player.whoAmI, 0, white ? 1 : 0);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(0.85f, 1.15f), type, damage, knockback, player.whoAmI, 0, white ? 1 : 0);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, type, damage, knockback, player.whoAmI, 0, white ? 1 : 0);
            }
            else
            {
                TranscendenceUtils.ProjectileShotgun(velocity.RotatedBy(MathHelper.PiOver4 / 3f), position, source, type, damage, knockback, 1, 3, 15, player.whoAmI, 0, 1, 0, -1);
                Combo = 0;
            }
            Combo++;
            return false;
        }
    }
}