using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Consumables.Boss
{
    public class CosmicArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 18;

            Item.value = Item.buyPrice(gold: 25);
            Item.rare = ModContent.RarityType<MidnightBlue>();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<AetherRootItem>(), 18)
            .AddIngredient(ModContent.ItemType<PulverizedPlanet>(), 16)
            .AddIngredient(ModContent.ItemType<SoulOfKnight>(), 5)
            .AddIngredient(ModContent.ItemType<HeartOfTheQueen>())
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
