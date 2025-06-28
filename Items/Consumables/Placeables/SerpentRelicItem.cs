using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Tiles.BigTiles.Decorations;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class SerpentRelicItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MoonLordMasterTrophy);
            Item.rare = ItemRarityID.Master;
            Item.DefaultToPlaceableTile(ModContent.TileType<SerpentRelic>(), 0);
        }
    }
}
