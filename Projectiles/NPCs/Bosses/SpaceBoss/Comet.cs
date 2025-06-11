using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class Comet : ModProjectile
    {
        public int Timer;
        public float Rotation;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 134;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.hostile = true;
            Projectile.tileCollide = false;

            Projectile.ignoreWater = true;
            Projectile.light = 0.25f;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            
            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.Center, 16, 16, DustID.TintableDustLighted, 0, 0, 0, Color.White, 0.75f);

            if (++Timer > 3 && Projectile.ai[2] != 0)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (360 * Projectile.ai[2])) * 1.05f;
                Timer = 0;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver4) * 40) < 12)
                return true;
            else
                return false;
        }
        public override void PostDraw(Color lightColor)
        {
            
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, 1.25f, "TranscendenceMod/Miscannellous/Assets/Bloom", Projectile.rotation, Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver4) * 40, null);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.Gray, 1f, Texture, false, true, 2f, Vector2.Zero);

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, 1f, Texture, Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}