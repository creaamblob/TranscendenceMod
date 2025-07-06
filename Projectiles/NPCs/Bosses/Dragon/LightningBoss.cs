using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class LightningBoss : ModProjectile
    {
        public int Timer;
        Vector2 vel;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 300;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            AIType = ProjectileID.Bullet;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            SoundEngine.PlaySound(SoundID.Thunder);
            vel = Projectile.velocity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            if (Projectile.localAI[1] < 15)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                TranscendenceUtils.DrawEntity(Projectile, Color.Aqua, 1f, "TranscendenceMod/Miscannellous/Assets/BloomLine", vel.ToRotation() + MathHelper.PiOver2, Projectile.Center, null);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                return false;
            }

            return true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void AI()
        {
            Projectile.ai[2]--;
            if (++Projectile.localAI[1] > 15)
                Projectile.velocity = vel = vel * 1.025f;
            else if (Projectile.localAI[1] > 1 && Projectile.localAI[1] < 15)
                Projectile.velocity = Vector2.Zero;
            Projectile.spriteDirection = (int)Projectile.velocity.ToRotation();
            /*if (Projectile.velocity == Vector2.Zero && Projectile.timeLeft < 875)
                Projectile.Kill();*/
        }
    }
}