using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class Lumimyrsky : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 36;
            Item.mana = 10;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item109;

            Item.width = 17;
            Item.height = 31;

            Item.useTime = 12;
            Item.useAnimation = 12;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;

            Item.shoot = ModContent.ProjectileType<LumimyrskyHiutale>();
            Item.shootSpeed = 15f;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4 - (0.3f * player.direction)) * 55;
        }
    }
}