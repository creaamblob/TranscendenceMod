using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class StormbowArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.timeLeft = 900;
            Projectile.penetrate = 15;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity.X = Projectile.DirectionTo(Main.MouseWorld).X * 17;
            TranscendenceUtils.DustRing(Projectile.Center, 7, DustID.YellowStarDust, 3, Color.White, 1.25f);
        }

        public override void AI()
        {
            if (++Projectile.ai[2] > 180 && !Collision.SolidCollision(Projectile.Center, Projectile.width * 2, Projectile.height * 2))
                Projectile.tileCollide = true;
            Projectile.velocity.Y += 8;
        }

        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 7, DustID.YellowStarDust, 3, Color.White, 1.25f);
            return base.PreKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 0.8f, $"{Texture}", false, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}