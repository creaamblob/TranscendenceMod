using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class ShimmerBlossomProj : ModProjectile
    {
        public int ExplosionCD;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 36;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.timeLeft = 300;
            Projectile.penetrate = 10;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 35, ModContent.DustType<Rainbow>(), 8, Color.White, 1.5f);
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.25f;
            if (ExplosionCD > 0) ExplosionCD--;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
            if (ExplosionCD < 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<BlossomBoom>(), (int)(Projectile.damage * 1.5f), 5, Main.player[Projectile.owner].whoAmI);
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                ExplosionCD = 10;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 0.8f, $"{Texture}", true, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}