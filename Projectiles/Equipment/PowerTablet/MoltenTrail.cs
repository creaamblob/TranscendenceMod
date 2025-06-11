using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
namespace TranscendenceMod.Projectiles.Equipment.PowerTablet
{
    public class MoltenTrail : ModProjectile
    {
        public float Fade = 1f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 24;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;

            Projectile.timeLeft = 240;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            NPC n = Projectile.FindTargetWithinRange(500);

            if (n != null && n.active && n.CanBeChasedBy())
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(n.Center) * 12f, 0.05f);

            if (Projectile.timeLeft < 30)
                Fade -= 1 / 30f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 5)
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