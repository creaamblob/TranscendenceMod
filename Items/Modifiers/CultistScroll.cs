using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Tiles.TerrestrialSecond;

namespace TranscendenceMod.Items.Modifiers
{
    public class CultistScroll : BaseModifier
    {
        public override int RequiredItem => ItemID.Ectoplasm;
        public override int RequiredAmount => 4;
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.CultScroll");
        public override ModifierIDs ModifierType => ModifierIDs.CultistScroll;
        public override bool CanBeApplied(Item item) => item.accessory;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 26;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Cyan;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Silk, 6)
            .AddIngredient(ItemID.Ectoplasm, 8)
            .AddIngredient(ItemID.LunarTabletFragment, 8)
            .AddTile(ModContent.TileType<ExtraTerrestrialLoom>())
            .Register();
        }
    }
}
