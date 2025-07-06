using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class LostHead : ModItem
    {
        public override string Texture => "TranscendenceMod/NPCs/Miniboss/HeadlessZombie_Head_Boss";
        int proj = ModContent.ProjectileType<LostHeadProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 18;
            Item.mana = 3;
            Item.knockBack = 2;

            Item.width = 17;
            Item.height = 31;

            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = proj;
            Item.shootSpeed = 1;
            Item.channel = true;
            Item.UseSound = SoundID.Item13;
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[proj] == 0;
        }
    }
}