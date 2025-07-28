using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class DysphoricYoyoProj : ModProjectile
    {
        public int Timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 400;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 25;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = 99;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
        }
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(1.1f) * 1.2f;
            Projectile.velocity.Y -= Main.rand.NextFloat(-25f, 25f);

            Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Phantasmal, Vector2.Zero, 0, Color.White, Projectile.scale);
            d.noGravity = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}