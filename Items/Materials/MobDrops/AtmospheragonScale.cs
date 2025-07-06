using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Materials.MobDrops
{
    public class AtmospheragonScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ModContent.RarityType<Brown>();
            Item.maxStack = 9999;
        }
    }
}
