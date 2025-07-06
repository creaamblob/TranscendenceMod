using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Modifiers
{
    public class BoltProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;

            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 13;
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(2.5f);
        }
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 10, DustID.TheDestroyer, Color.White, 1.25f, 2f, 4f);
            return base.PreKill(timeLeft);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.DarkGray, Projectile.scale, $"{Texture}", true, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}