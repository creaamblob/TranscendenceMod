using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Modifiers
{
    public class GiantHandle : BaseModifier
    {
        public override int RequiredItem => ItemID.LifeCrystal;
        public override int RequiredAmount => 4;
        public override ModifierIDs ModifierType => ModifierIDs.GiantHandle;
        public override bool CanBeApplied(Item item) => item.DamageType == DamageClass.Melee && item.useStyle == ItemUseStyleID.Swing && item.axe == 0 && item.pick == 0 || item.GetGlobalItem<ModifiersItem>().BlacksmithGiantHandleAllowed == true;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 17;
            Item.height = 17;
            Item.value = Item.buyPrice(gold: 40);
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.LifeCrystal, 5)
            .AddIngredient(ModContent.ItemType<HardmetalBar>(), 10)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
