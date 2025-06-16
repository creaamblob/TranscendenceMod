using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged.Ammo
{
    public class ShimmerBulletProj : ModProjectile
    {
        public Vector2 intialVel;
        Color rainbow = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, byte.MaxValue);
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return rainbow;
        }
        public override void OnSpawn(IEntitySource source)
        {
            intialVel = Projectile.velocity;
        }
        public override void AI()
        {
            if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.velocity = intialVel;
                if (++Projectile.ai[2] > 5)
                    Projectile.tileCollide = true;
            }
            else Projectile.velocity = intialVel * 0.125f;
            if (Main.rand.NextBool(4))
            {
                Dust rainbowDust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(4f, 4f), DustID.SparkForLightDisc, new Vector2?(Projectile.velocity * 1.25f), 0, rainbow, 1f + Main.rand.NextFloat() * 0.3f);
                rainbowDust.noGravity = true;
                rainbowDust.scale += 0.05f;

                Dust rainbowDust2 = Dust.CloneDust(rainbowDust);
                rainbowDust2.color = Color.White;
                rainbowDust2.scale -= 0.1f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.ShimmerTownNPCSend, Projectile.Center, -1);
            SoundEngine.PlaySound(SoundID.Item176, Projectile.Center);
            Projectile.ai[2] = 0;
            Projectile.tileCollide = false;
            return false;
        }
        public override void OnKill(int timeLeft) => SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
    }
}