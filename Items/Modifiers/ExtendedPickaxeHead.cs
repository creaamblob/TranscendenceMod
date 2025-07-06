using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Items.Modifiers
{
    public class ExtendedPickaxeHead : BaseModifier
    {
        public override int RequiredItem => ModContent.ItemType<HardmetalOre>();
        public override int RequiredAmount => 6;
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.ExtendedHead");
        public override ModifierIDs ModifierType => ModifierIDs.LongPickHead;
        public override bool CanBeApplied(Item item) => item.pick > 0 && item.useStyle == ItemUseStyleID.Swing;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 17;
            Item.height = 17;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<CarbonBar>(), 12)
            .AddRecipeGroup(RecipeGroupID.IronBar, 6)
            .AddIngredient(ItemID.Bone, 30)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
