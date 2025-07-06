using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class CannonBall : ModProjectile
    {
        NPC victim;
        public int HomeCD;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
            ProjectileID.Sets.SentryShot[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;

            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.ArmorPenetration = 10;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
        }
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 15, DustID.Torch, 5, Color.White, 5);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }
        public override void AI()
        {
            Projectile.rotation += 0.005f;
            if (Projectile.timeLeft < 285)
                Projectile.velocity.Y += 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, lightColor, Projectile.scale, $"{Texture}", false, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}