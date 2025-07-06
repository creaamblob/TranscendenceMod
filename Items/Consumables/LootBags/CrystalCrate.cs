using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles.Decorations;
using Microsoft.Xna.Framework;
using TranscendenceMod.Items.Consumables.FoodAndDrinks;

namespace TranscendenceMod.Items.Consumables.LootBags
{
    public class CrystalCrate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
            ItemID.Sets.IsFishingCrate[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<MidnightBlue>();
            Item.maxStack = 9999;

            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<CrystalCrate_Tile>();
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Crates;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SpaceRockItem>(), 1, 5, 10));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AetherRootItem>(), 1, 4, 9));
            itemLoot.Add(ItemDropRule.Common(ItemID.Meteorite, 2, 4, 16));
            itemLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 4, 5, 12));

            IItemDropRule[] potions = new IItemDropRule[]
            {
                ItemDropRule.Common(ModContent.ItemType<ExtraTerrestrialBrew>(), 2, 1, 2),
                ItemDropRule.Common(ModContent.ItemType<StarcraftedStew>(), 3),
                ItemDropRule.Common(ItemID.GravitationPotion, 2, 1, 2),
                ItemDropRule.Common(ItemID.MagicPowerPotion, 1, 1, 2),
                ItemDropRule.Common(ItemID.ManaRegenerationPotion, 1, 1, 2),
                ItemDropRule.Common(ItemID.TeleportationPotion, 2),
                ItemDropRule.Common(ItemID.PotionOfReturn, 2, 1, 2)
            };
            itemLoot.Add(new OneFromRulesRule(2, potions));

            IItemDropRule[] bait = new IItemDropRule[]
            {
                ItemDropRule.Common(ModContent.ItemType<EnchantedHopper>(), 1, 1, 3),
                ItemDropRule.Common(ItemID.EnchantedNightcrawler, 1, 1, 2),
                ItemDropRule.Common(ItemID.JourneymanBait, 2, 1, 3),
                ItemDropRule.Common(ItemID.MasterBait, 3, 1, 2)
            };
            itemLoot.Add(new OneFromRulesRule(3, bait));
        }
        public override bool CanRightClick() => true;
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
