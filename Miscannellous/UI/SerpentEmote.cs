using Terraria.GameContent.UI;
using Terraria.ModLoader;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.Miscannellous.UI
{
    public class SerpentEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.Dangers);
        }

        public override bool IsUnlocked() => Downed.Contains(Bosses.FrostSerpent);
    }
}
