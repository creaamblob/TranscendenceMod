using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class SewerHoop : ModItem
    {
        public int boomerang = ModContent.ProjectileType<SewerHoopProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.damage = 255;

            Item.knockBack = 2.25f;
            Item.width = 18;
            Item.height = 18;

            Item.shoot = boomerang;
            Item.shootSpeed = 10;

            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
            Item.crit = 5;
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[boomerang] < 3;
        }
    }
}