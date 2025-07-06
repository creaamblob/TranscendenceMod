using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class TinkererGunBullet : ModProjectile
    {
        public float scale;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 450;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            if (scale < 1)
                scale += 0.025f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;

            TranscendenceUtils.AnimateProj(Projectile, 6);
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, scale, $"{Texture}", Projectile.rotation, new Vector2(0, -5), true, false, true);
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, scale, $"{Texture}", Projectile.rotation, Projectile.Center, false, false, false);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 180);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            TranscendenceUtils.DustRing(Projectile.Center, 20, DustID.AmberBolt, 2.5f, Color.Red, 2);
        }
    }
}