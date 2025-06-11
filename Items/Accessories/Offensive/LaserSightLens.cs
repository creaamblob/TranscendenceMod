using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Offensive
{
    public class LaserSightLens : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.height = 18;
            Item.width = 18;

            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ModContent.RarityType<Brown>();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().NucleusLensSocial = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().NucleusLens = true;
            if (!hideVisual)
                player.GetModPlayer<TranscendencePlayer>().NucleusLensSocial = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ReconScope)
            .AddIngredient(ItemID.RangerEmblem)
            .AddIngredient(ItemID.UltrabrightHelmet)
            .AddIngredient(ModContent.ItemType<ElectricalComponent>(), 2)
            .AddIngredient(ItemID.SoulofSight, 15)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
