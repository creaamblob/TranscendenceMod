using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items
{
    public class NohitMode : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 26;
            Item.rare = ItemRarityID.LightRed;
        }
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);

            if (Item.favorited)
                player.GetModPlayer<TranscendencePlayer>().NohitMode = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LeadBar, 14)
            .AddIngredient(ItemID.WormTooth, 6)
            .AddIngredient(ItemID.Ruby, 4)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
