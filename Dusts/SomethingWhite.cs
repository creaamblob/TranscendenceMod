using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class SomethingWhite : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
        }
    }
}
