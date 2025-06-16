using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Pumpkin : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = Main.rand.Next(1, 2);
            dust.velocity *= 0.3f;
        }
    }
}
