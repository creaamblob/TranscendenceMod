using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Potions;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Consumables.FoodAndDrinks
{
    public class StarcraftedBooze : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Ale);
            Item.buffTime = 6 * 60 * 60;
            Item.buffType = ModContent.BuffType<ExtraTerrestrialBuff>();
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.buyPrice(gold: 1, silver: 50);
            Item.rare = ModContent.RarityType<ModdedPurple>();
        }
    }
}
