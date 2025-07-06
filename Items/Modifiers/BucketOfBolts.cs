using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace TranscendenceMod.Items.Modifiers
{
    public class BucketOfBolts : BaseModifier
    {
        public override int RequiredItem => ItemID.HallowedBar;
        public override int RequiredAmount => 5;
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.Mechanical");
        public override ModifierIDs ModifierType => ModifierIDs.Mechanical;
        public override bool CanBeApplied(Item item) => item.legSlot != -1;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 24;
            Item.height = 16;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.Pink;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 15)
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.SoulofSight, 3)
            .AddIngredient(ItemID.SoulofMight, 3)
            .AddIngredient(ItemID.SoulofFright, 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
