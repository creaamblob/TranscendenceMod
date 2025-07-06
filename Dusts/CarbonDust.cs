using Microsoft.Xna.Framework;
using Terraria;

namespace TranscendenceMod.Dusts
{
    public class CarbonDust : GravityCollisionDust
    {
        public override int TimeUntilFade => 180;

        public override int FadeSpeed => 2;

        public override float MaxGrow => 0f;

        public override int HitboxSize => 5;

        public override float FallSpeed => 2;

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, Main.rand.NextBool(2) ? 10 : 0, 10, 10);
            dust.noGravity = true;
        }
    }
}
