using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class VoidDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor) => Color.White;
    }
}
