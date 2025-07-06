using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Ranged.Ammo;

namespace TranscendenceMod.Items.Weapons.Ranged.Ammo
{
    public class LuminiteStake : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 35;
            Item.ammo = AmmoID.Stake;
            Item.shoot = ModContent.ProjectileType<LuminiteStakeProj>();
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.width = 20;
            Item.height = 8;
            Item.value = Item.buyPrice(copper: 3);
            Item.rare = ItemRarityID.Cyan;
        }
        public override void AddRecipes()
        {
            CreateRecipe(333)
            .AddIngredient(ItemID.LunarBar, 1)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
