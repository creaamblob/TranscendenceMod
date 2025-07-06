using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class TurmoilMiniProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.penetrate = 2;
            Projectile.timeLeft = 60;
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity.Y += 0.5f;

            if (Projectile.timeLeft < 30)
                Projectile.scale -= 1f / 30f;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.DeepSkyBlue, Projectile.scale, $"{Texture}", true, true, Projectile.scale, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 6, ModContent.DustType<LandsiteDroplet>(), Color.White, 0.75f, 1.5f, 3f);
            SoundEngine.PlaySound(SoundID.Item95, Projectile.Center);
        }
    }
}