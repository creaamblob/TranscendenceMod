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
            Item.damage = 35;

            Item.knockBack = 3f;
            Item.width = 20;
            Item.height = 20;

            Item.shoot = boomerang;
            Item.shootSpeed = 14f;

            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.crit = 5;
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[boomerang] < 3;
        }
    }
}