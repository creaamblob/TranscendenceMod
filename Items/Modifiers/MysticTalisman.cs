using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Modifiers
{
    public class MysticTalisman : BaseModifier
    {
        public override int RequiredItem => ItemID.DefenderMedal;
        public override int RequiredAmount => 25;
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.Mystic");
        public override ModifierIDs ModifierType => ModifierIDs.Mystic;
        public override bool CanBeApplied(Item item) => item.type == ItemID.ApprenticeAltHead || item.type == ItemID.ApprenticeAltShirt || item.type == ItemID.ApprenticeAltPants;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 12;
            Item.height = 12;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ModContent.RarityType<ModdedPurple>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.DefenderMedal, 50)
            .AddIngredient(ItemID.Ectoplasm, 25)
            .AddIngredient(ItemID.SoulofNight, 20)
            .AddIngredient(ItemID.LunarBar, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
