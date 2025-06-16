using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Envelopes
{
    public class WallOfFleshNote : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 16;

            Item.rare = ItemRarityID.Pink;
        }
    }
}
