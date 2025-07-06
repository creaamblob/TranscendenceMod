using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Materials.MobDrops
{
    public class MosquitoVenom : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 23;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 9999;
        }
    }
}
