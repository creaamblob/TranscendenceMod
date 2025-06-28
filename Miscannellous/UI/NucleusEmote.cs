using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.UI
{
    public class NucleusEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.Dangers);
        }

        public override bool IsUnlocked() => TranscendenceWorld.DownedNucleus;
    }
}
