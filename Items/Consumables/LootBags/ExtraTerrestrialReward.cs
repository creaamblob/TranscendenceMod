using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Accessories.Shields;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Consumables.LootBags
{
    public class ExtraTerrestrialReward : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.width = 32;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Orange;
            Item.maxStack = 9999;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int box = ModContent.ItemType<PortalBox>();
            int shield = ModContent.ItemType<StardustShieldGenerator>();

            IItemDropRule[] Generic = new IItemDropRule[] {

                ItemDropRule.Common(ItemID.MeteoriteBar, 3, 9, 15),
                ItemDropRule.Common(ModContent.ItemType<PulverizedPlanet>(), 2, 7, 11),

                ItemDropRule.Common(ModContent.ItemType<SpaceRockItem>(), 2, 7, 22),
                ItemDropRule.Common(ModContent.ItemType<AetherRootItem>(), 2, 5, 16),

                ItemDropRule.Common(ModContent.ItemType<FlowerEssence>(), 3, 1, 2)

            };

            ItemDropRule.Coins(Main.rand.Next(Item.sellPrice(gold: 4, silver: 25), Item.sellPrice(gold: 8, silver: 50)), false);

            //Add Generic Loot
            itemLoot.Add(new AlwaysAtleastOneSuccessDropRule(Generic));
            itemLoot.Add(new AlwaysAtleastOneSuccessDropRule(Generic));

            itemLoot.Add(new OneFromOptionsDropRule(3, 1, box, shield));
        }
        public override bool CanRightClick() => true;
    }
}
