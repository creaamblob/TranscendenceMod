using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Weapons.Ranged.Ammo
{
    public class LivingBulletProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 600;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0, 0.3f, 0.15f);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, -Projectile.velocity, ModContent.ProjectileType<LivingBulletTree>(),
                (int) (Projectile.damage * 0.066f), 5, Main.player[Projectile.owner].whoAmI);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
    }
}