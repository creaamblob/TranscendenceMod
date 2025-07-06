using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class ArcadeCabinetItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ModContent.RarityType<Brown>();
            Item.DefaultToPlaceableTile(ModContent.TileType<ArcadeCabinet>());
        }
    }
}
