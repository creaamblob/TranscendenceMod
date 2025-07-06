using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class DreadshotBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 450;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.extraUpdates = 1 + (int)Projectile.ai[1];
            Projectile.damage *= (int)(1 + Projectile.ai[1]);
            Projectile.penetrate = 1 + (int)(Projectile.ai[1] * 5);
            ProjectileID.Sets.TrailCacheLength[Type] = 1 + (int)(Projectile.ai[1] * 120);
            Projectile.Opacity = 0;
        }
        public override void AI()
        {
            if (Projectile.Opacity < 1)
                Projectile.Opacity += 0.0125f;

            Lighting.AddLight(Projectile.Center, 0.2f, 0.125f, 0f);
            if (Main.rand.NextBool(21 - (int)(Projectile.ai[1] * 10)))
                Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<BetterBlood>(), 0, 0, 0, default, 0.5f);
        }
        public override void OnKill(int timeLeft) => SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;

            TranscendenceUtils.DrawTrailProj(Projectile, Color.Red * Projectile.Opacity, 1f + (1 * (Projectile.ai[1] * 1.125f)), $"Terraria/Images/Projectile_977", false, true, 1.25f, Vector2.Zero);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White * Projectile.Opacity, 0.55f + (0.25f * (Projectile.ai[1] * 1f)), "TranscendenceMod/Miscannellous/Assets/Trail2", false, true, 1.125f, Vector2.Zero);
            return false;
        }
    }
}