using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class NucleusSummonerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<Brown>();
            Item.DefaultToPlaceableTile(ModContent.TileType<NucleusSummoner>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 30)
            .AddIngredient(ModContent.ItemType<AtmospheragonScale>(), 6)
            .AddIngredient(ModContent.ItemType<Lightning>(), 8)
            .AddIngredient(ItemID.Wire, 999)
            .AddIngredient(ItemID.SoulofSight, 5)
            .AddIngredient(ItemID.SoulofMight, 5)
            .AddIngredient(ItemID.SoulofFright, 5)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
