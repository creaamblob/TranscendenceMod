using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscanellous.MiscSystems
{
    // Hitler Dead
    public class FishronPerceptionSystem : ModSystem
    {
        public bool ShouldBeActive => Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().FishronPerceptionAcc;
        public override void Load()
        {
            On_Main.DrawWaters += DontDrawLiquid;
        }

        private void DontDrawLiquid(On_Main.orig_DrawWaters orig, Main self, bool isBackground)
        {
            if (!ShouldBeActive)
                orig(self, isBackground);
        }
    }
}