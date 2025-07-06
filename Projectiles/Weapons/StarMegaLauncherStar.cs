using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons
{
    public class StarMegaLauncherStar : ModProjectile
    {
        public Vector2 vel;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 95;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source) => vel = Projectile.velocity;
        public override void AI()
        {
            if (Projectile.ai[1] < 30)
                Projectile.velocity *= MathHelper.Lerp(1f, 0.5f, Projectile.ai[1] / 30f);
            if (++Projectile.ai[1] == 30)
            {
                TranscendenceUtils.ProjectileShotgun(vel, Projectile.Center, Projectile.GetSource_FromAI(), ModContent.ProjectileType<StarMegaLauncherDeathray>(), Projectile.damage, 1, 1, 2, 45, Main.player[Projectile.owner].whoAmI, 2, 0, 0, 0);
                //TranscendenceUtils.ProjectileShotgun(-vel.RotatedBy(1), Projectile.Center, Projectile.GetSource_FromAI(), ModContent.ProjectileType<StarMegaLauncherDeathray>(), Projectile.damage, 1, 1, 2, 10, Main.player[Projectile.owner].whoAmI, 1, 0, 0, 0);
            }
            if (Projectile.timeLeft < 25) Projectile.scale -= 0.04f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, $"{Texture}", false, true, 1, Vector2.Zero);
            TranscendenceUtils.DrawEntity(Projectile, Color.OrangeRed, Projectile.scale * 3f, "bloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Yellow, 0.75f * Projectile.scale * 2f, "bloom", 0, Projectile.Center, null);
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.ParticleOrchestra(Terraria.GameContent.Drawing.ParticleOrchestraType.Excalibur, Projectile.Center, -1);
        }
    }
}