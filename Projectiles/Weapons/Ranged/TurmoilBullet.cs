using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class TurmoilBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 450;
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity.Y += 0.33f;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.DeepSkyBlue, 1, $"{Texture}", true, true, 1, Vector2.Zero);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return base.PreDraw(ref lightColor);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-16f, -12f)),
                ModContent.ProjectileType<TurmoilMiniProj>(), Projectile.damage / 5, Projectile.knockBack / 2, Main.player[Projectile.owner].whoAmI);
        }
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 12, ModContent.DustType<LandsiteDroplet>(), Color.White, 1f, 2f, 4.25f);
            SoundEngine.PlaySound(SoundID.Item95, Projectile.Center);
        }
    }
}