using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
namespace TranscendenceMod.Projectiles.Equipment.PowerTablet
{
    public class MoltenSlash : ModProjectile
    {
        public float Fade = 1f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 16;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 5;

            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            if (Projectile.timeLeft < 30)
                Fade -= 1 / 30f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (20 * Projectile.scale))
                return true;
            return base.Colliding(projHitbox, targetHitbox);
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