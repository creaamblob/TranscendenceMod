using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Materials.MobDrops
{
    public class MosquitoLeg : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 7;
            Item.height = 23;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Lime;
            Item.maxStack = 9999;
        }
    }
}
