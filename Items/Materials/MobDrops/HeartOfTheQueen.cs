using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Materials.MobDrops
{
    public class HeartOfTheQueen : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 12, silver: 50);
            Item.rare = ItemRarityID.Red;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
