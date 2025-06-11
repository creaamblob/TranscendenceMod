using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
namespace TranscendenceMod.Projectiles.Equipment.PowerTablet
{
    public class MoltenBullet : ModProjectile
    {
        public float Fade = 1f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 20;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;

            Projectile.timeLeft = 450;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 30)
                Fade -= 1 / 30f;
            Projectile.velocity = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel.RotatedByRandom(0.75f);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (20 * Projectile.scale))
                return true;
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);

            for (int i = 0; i < 6; i++)
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<ArenaDust>(), Main.rand.NextVector2Circular(2f, 2f), 0, Color.OrangeRed, 0.75f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White * Fade, Projectile.scale * Fade, Texture, false, true, 1, Vector2.Zero);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            return false;
        }
    }
}