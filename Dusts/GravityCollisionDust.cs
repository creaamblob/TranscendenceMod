using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public abstract class GravityCollisionDust : ModDust
    {
        public abstract int TimeUntilFade { get; }
        public abstract int FadeSpeed { get; }
        public abstract float MaxGrow { get; }
        public abstract int HitboxSize { get; }
        public abstract float FallSpeed { get; }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;

            if (++dust.fadeIn > TimeUntilFade)
            {
                dust.alpha += FadeSpeed;
                if (dust.alpha > 244)
                    dust.active = false;
            }

            if (Collision.SolidCollision(dust.position - new Vector2(HitboxSize / 2f), HitboxSize, HitboxSize, true))
            {
                dust.velocity *= 0.25f;
                if (dust.scale < MaxGrow && dust.scale > 0.25f) dust.scale *= 1.075f;
            }
            else
            {
                dust.velocity.Y += FallSpeed;
                dust.scale *= 0.985f;
            }
            if (dust.scale < 0.05f)
                dust.active = false;
            return false;
        }
    }
}
