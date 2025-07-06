using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Armor.Hats;
using TranscendenceMod.Items.Consumables.Placeables.Decorations;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Items.Consumables.LootBags
{
    public class NucleusBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BossBag[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Expert;

            Item.expert = true;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.value = 0;
            Item.maxStack = 9999;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfKnight>(), 1, 12, 20));
            itemLoot.Add(ItemDropRule.Common(ItemID.HallowedBar, 1, 15, 30));
            itemLoot.Add(ItemDropRule.Common(ItemID.LunarOre, 1, 45, 60));

            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<NucleusMask>(), 7));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<NucleusTrophyItem>(), 10));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<ProjectNucleus>()));
        }
        public override bool CanRightClick()
        {
            return true;
        }
    }
}
