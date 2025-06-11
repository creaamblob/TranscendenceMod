using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class Rock : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 600;
            Projectile.ArmorPenetration = 5;
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation += 3;
            Projectile.velocity.Y += 0.25f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, lightColor, 1, $"{Texture}", true, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++) Dust.NewDust(Projectile.Center, 8, 8, DustID.Stone);
            SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);
        }
    }
}