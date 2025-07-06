using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Other
{
    public class PocketGuillotine : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 14;
            Item.width = 14;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().PocketGuillotine = true;
        }
    }
}
