using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Projectiles.Weapons.Ranged.Ammo
{
    public class ToxicArrowProj : ModProjectile
    {
        public int BounceCD = 0;
        public int Bounces;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.timeLeft = 900;
            Projectile.penetrate = 5;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 12, DustID.ChlorophyteWeapon, 3, Color.White, 1.25f);
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.125f;
            if (BounceCD > 0)
                BounceCD--;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
            SoundEngine.PlaySound(SoundID.Grass with { MaxInstances = 0});
            TranscendenceUtils.DustRing(Projectile.Center, 6, DustID.Chlorophyte, 2, Color.White, 1f);
            int gas = ModContent.ProjectileType<ToxicArrowImpact>();
            if (BounceCD == 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center,
Vector2.Zero, gas,
(int)(Projectile.damage * 0.25f), 0, Main.player[Projectile.owner].whoAmI);
            }
            BounceCD = 60;
            if (++Bounces > 1) Projectile.Kill();
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, Texture, true, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}