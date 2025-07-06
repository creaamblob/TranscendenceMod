using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles.Decorations;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class SeraphMonolithItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.VoidMonolith);
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ModContent.RarityType<MidnightBlue>();
            Item.accessory = true;
            Item.DefaultToPlaceableTile(ModContent.TileType<SeraphMonolith>());
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().ZoneSeraphMonolith = true;
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().ZoneSeraphMonolith = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<SpaceRockItem>(), 15)
            .AddIngredient(ModContent.ItemType<ShimmerChunk>(), 6)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
