using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Modifiers.Upgrades
{
    public class SpaceScrap : BaseModifier
    {
        public override bool CanBeApplied(Item item) => item.type == ItemID.RocketLauncher;
        public override int RequiredItem => ModContent.ItemType<CrystalItem>();
        public override int RequiredAmount => 20;
        public override ModifierIDs ModifierType => ModifierIDs.CosmicCrystal;
        public override int CraftingResultItem => ModContent.ItemType<CosmosShardLauncher>();
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.CosmicCrystal");
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 24;
            Item.height = 24;

            Item.value = Item.buyPrice(gold: 35);
            Item.rare = ModContent.RarityType<MidnightBlue>();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}