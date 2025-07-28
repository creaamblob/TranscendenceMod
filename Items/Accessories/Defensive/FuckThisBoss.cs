using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Defensive
{
    public class FuckThisBoss : ModItem
    {
        // This fucker has made quit so many playthroughs
        // Fuck Moon Lord for real
        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.width = 12;
            Item.height = 16;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().FairerMoonlord = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ShimmerCloak)
            .AddIngredient(ItemID.FragmentSolar, 8)
            .AddIngredient(ItemID.FragmentVortex, 8)
            .AddIngredient(ItemID.FragmentNebula, 8)
            .AddIngredient(ItemID.FragmentStardust, 8)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
