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
            Item.damage = 32;
            Item.mana = 5;
            Item.knockBack = 1;

            Item.width = 17;
            Item.height = 31;

            Item.useTime = 4;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.shoot = ModContent.ProjectileType<LumimyrskyHiutale>();
            Item.shootSpeed = 15;
            Item.UseSound = SoundID.Item109;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4 - (0.3f * player.direction)) * 55;
        }
        /*public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4f, 0f);
        }*/
    }
}