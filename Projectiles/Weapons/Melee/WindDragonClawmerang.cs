using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class WindDragonClawmerang : ModProjectile
    {
        public float Speed = 1.025f;
        public int Timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 48;
            Projectile.ownerHitCheck = true;

            Projectile.timeLeft = 180;
            Projectile.aiStyle = 1;
            Projectile.penetrate = 15;

            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            //Projectile.velocity *= 1.01f;
            //Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / 180) * 1.05f;
            Player player = Main.player[Projectile.owner];
            if (++Projectile.ai[2] < 3)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (180 * Projectile.ai[1])) * Speed;
                Projectile.ai[2] = 0;
            }

            if (Timer == 5) Projectile.tileCollide = true;

            if (Timer == 15)
                Projectile.ai[1] = -Projectile.ai[1];

            else if (Timer > 45) Projectile.velocity = Projectile.DirectionTo(player.Center) * 35;
            if (++Timer > 45 && Projectile.Distance(player.Center) < 50)
                Projectile.Kill();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Timer < 46)
                Timer = 46;
            return Timer > 50;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, lightColor, Projectile.scale, $"{Texture}", true, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.SilverBulletSparkle, Projectile.Center, Projectile.whoAmI);
            return base.PreKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
            {
                TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.SilverBulletSparkle, Projectile.Center, Projectile.whoAmI);
            }
        }
    }
}