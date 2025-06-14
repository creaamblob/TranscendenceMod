using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent
{
    public class MagicalSnowflake : ModProjectile
    {
        public float Fade;
        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override bool CanHitPlayer(Player target)
        {
            return Fade > 0.9f && Projectile.scale > 0.9f && Projectile.ai[2] > 90;
        }
        public override void AI()
        {
            if (++Projectile.ai[2] > 30)
            {
                Projectile.velocity *= 0.95f;

                if (Fade < 1f)
                    Fade += 0.075f;
            }
            if (Projectile.timeLeft < 30 && Projectile.scale > 0f)
                Projectile.scale -= 1f / 30f;

            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().ModUnparryable = !(Fade > 0.85f && Projectile.scale > 0.9f);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.Blue * 0.66f, 3f * Projectile.scale, "bloom", 0, Projectile.Center, null);

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2, 1f, 1f, 1f, 1f, false);

            TranscendenceUtils.DrawEntity(Projectile, Color.Lerp(Color.Blue * 0.33f, Color.White, Fade), Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}