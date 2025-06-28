using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Accessories.Shields;

namespace TranscendenceMod.Items.Consumables.LootBags
{
    public class CrystalReward : ModItem
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
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.maxStack = 9999;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int box = ModContent.ItemType<PortalBox>();
            int shield = ModContent.ItemType<StardustShieldGenerator>();

            int rock = ModContent.ItemType<SpaceRockItem>();
            int root = ModContent.ItemType<AetherRootItem>();
            int crystal = ModContent.ItemType<CrystalItem>();

            IItemDropRule[] Generic = new IItemDropRule[] {
                ItemDropRule.Common(ItemID.MeteoriteBar, 2, 5, 12),
                ItemDropRule.Common(ModContent.ItemType<SunBar>(), 2, 4, 8),

                ItemDropRule.Common(ModContent.ItemType<FlowerEssence>(), 2, 1, 2),
            };
            IItemDropRule[] Tiles = new IItemDropRule[] {
                ItemDropRule.Common(rock, 2, 6, 12),
                ItemDropRule.Common(root, 2, 6, 12),
                ItemDropRule.Common(crystal, 3, 3, 7),
            };

            ItemDropRule.Coins(Main.rand.Next(Item.sellPrice(gold: 7, silver: 50), Item.sellPrice(gold: 12, silver: 25)), false);

            itemLoot.Add(new AlwaysAtleastOneSuccessDropRule(Generic));
            itemLoot.Add(new AlwaysAtleastOneSuccessDropRule(Generic));

            itemLoot.Add(new AlwaysAtleastOneSuccessDropRule(Tiles));
            itemLoot.Add(new OneFromOptionsDropRule(3, 1, box, shield));
        }
        public override bool CanRightClick() => true;
    }
}
