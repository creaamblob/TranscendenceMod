using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class RuneStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 66;
            Item.mana = 12;
            Item.knockBack = 2;

            Item.width = 17;
            Item.height = 31;

            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ModContent.ProjectileType<RuneBlastFriendly>();
            Item.shootSpeed = 15;
            Item.UseSound = SoundID.Item43;
        }
        public override bool AltFunctionUse(Player player) => true;
    }
}