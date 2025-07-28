using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables;

namespace TranscendenceMod.Items.Materials
{
    public class EtheriaDial : TerrariaDial
    {
        public override bool CanBeApplied(Item item) => item.type == ModContent.ItemType<TerrariaDial>();
        public override int RequiredItem => ModContent.ItemType<ConstantDial>();
        public override int RequiredAmount => 1;
        public override ModifierIDs ModifierType => ModifierIDs.TimedialEtheria;
        public override int CraftingResultItem => ModContent.ItemType<Timedial>();
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Purple;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            double curTime = Math.Round(Main.time / 60 + (Main.dayTime ? 0 : 900));
            string timeString = Language.GetTextValue("Mods.TranscendenceMod.Messages.Timedials", curTime.ToString());
            var time = new TooltipLine(Mod, "etheriaTime", timeString);
            tooltips.Add(time);
        }
    }
}
