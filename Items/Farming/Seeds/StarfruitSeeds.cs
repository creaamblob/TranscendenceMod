using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.TilesheetHell.Nature.Farming;

namespace TranscendenceMod.Items.Farming.Seeds
{
    public class StarfruitSeeds : BaseSeed
    {
        public override int Tile => ModContent.TileType<StarfruitCrop>();
        public override bool allowed(Tile tile2) => tile2.TileType == ModContent.TileType<StarSoil>();

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ModContent.RarityType<MidnightBlue>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
            .AddIngredient(ModContent.ItemType<Starfruit>())
            .AddIngredient(ModContent.ItemType<PulverizedPlanet>(), 4)
            .AddIngredient(ItemID.FallenStar, 3)
            .AddTile(TileID.Blendomatic)
            .Register();
        }
    }
}
