using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using System;

namespace TranscendenceMod.Projectiles.Weapons
{
    public class StarGunBullet : ModProjectile
    {
        public float scale;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.timeLeft = 300;

            Projectile.aiStyle = -1;

            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
                Projectile.ai[1] = 1;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (TranscendenceWorld.Timer % 3 == 0)
            Projectile.velocity = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel.RotatedBy(MathHelper.ToRadians((float)Math.Cos(TranscendenceWorld.Timer * 64f) * 90f * Projectile.ai[1])) * 0.5f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, $"{Texture}", false, true, 1, Vector2.Zero);
            TranscendenceUtils.DrawEntity(Projectile, Color.OrangeRed, 0.75f, "bloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.5f, "bloom", 0, Projectile.Center, null);
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            TranscendenceUtils.ParticleOrchestra(Terraria.GameContent.Drawing.ParticleOrchestraType.Excalibur, Projectile.Center, -1);
        }
    }
}