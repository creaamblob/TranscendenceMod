using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles.Decorations;

namespace TranscendenceMod.Items.Consumables.Placeables
{
    public class FrostMonolithItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.VoidMonolith);
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.accessory = true;
            Item.DefaultToPlaceableTile(ModContent.TileType<FrostMonolith>());
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().ZoneSerpentMonolith = 5;
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().ZoneSerpentMonolith = 5;
        }
    }
}
