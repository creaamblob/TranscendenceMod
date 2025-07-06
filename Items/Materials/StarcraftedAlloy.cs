using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Materials
{
    public class StarcraftedAlloy : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ModContent.RarityType<MidnightBlue>();
            Item.maxStack = 9999;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
