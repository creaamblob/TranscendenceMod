using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Envelopes
{
    public class WallOfFleshEnv : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 18;

            Item.rare = ItemRarityID.Pink;
        }
        public override void RightClick(Player player)
        {
            Item.NewItem(Item.GetSource_GiftOrReward(), player.getRect(), ModContent.ItemType<WallOfFleshNote>(), 1, false, 0, true);

            base.RightClick(player);
        }
        public override bool CanRightClick() => true;
    }
}
