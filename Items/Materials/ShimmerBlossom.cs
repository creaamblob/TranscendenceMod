using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Materials
{
    public class ShimmerBlossom : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Fireblossom);
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Expert;
            Item.maxStack = 9999;
        }
    }
}
