using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables;

namespace TranscendenceMod.Items.Materials
{
    public class ConstantDial : EarthDial
    {
        public override bool CanBeApplied(Item item) => item.type == ModContent.ItemType<EarthDial>();
        public override int RequiredItem => ModContent.ItemType<EtheriaDial>();
        public override int RequiredAmount => 1;
        public override ModifierIDs ModifierType => ModifierIDs.Timedial;
        public override int CraftingResultItem => ModContent.ItemType<Timedial>();
        public override string TooltipPath => Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.Modifiers.TimedialConstant");

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            string curTime = $"{Main.rand.Next(0, 23)}:{Main.rand.Next(0, 59)}";
            string timeString = Language.GetTextValue("Mods.TranscendenceMod.Messages.Timedials", curTime.ToString());
            var time = new TooltipLine(Mod, "constantTime", timeString);
            tooltips.Add(time);
        }
    }
}
