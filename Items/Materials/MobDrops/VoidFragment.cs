using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Materials.MobDrops
{
    public class VoidFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 9999;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
