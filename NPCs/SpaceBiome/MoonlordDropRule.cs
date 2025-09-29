using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using static TranscendenceMod.TranscendenceWorld;

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
        public bool CanDrop(DropAttemptInfo info) => Downed.Contains(Bosses.CelestialSeraph);
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => Language.GetTextValue("Mods.TranscendenceMod.Messages.SeraphCondition");
    }
}

