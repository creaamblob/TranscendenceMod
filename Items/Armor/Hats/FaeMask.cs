using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Armor.Hats
{
    [AutoloadEquip(EquipType.Head)]
    public class FaeMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}
