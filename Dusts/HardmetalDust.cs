using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class HardmetalDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.5f;
        }
    }
}
