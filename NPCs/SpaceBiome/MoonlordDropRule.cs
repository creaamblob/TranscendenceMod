using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class MoonlordDropRule : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => NPC.downedMoonlord;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => Language.GetTextValue("Mods.TranscendenceMod.Messages.MoonlordCondition");
    }
    public class SeraphDropRule : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => TranscendenceWorld.DownedSpaceBoss;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => Language.GetTextValue("Mods.TranscendenceMod.Messages.SeraphCondition");
    }
}

