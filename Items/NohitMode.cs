using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

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
            Item.height = 26;
            Item.width = 24;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);

            if (Item.favorited)
                player.GetModPlayer<TranscendencePlayer>().NohitMode = true;
        }
    }
}
