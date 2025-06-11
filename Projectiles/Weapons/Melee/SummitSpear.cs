using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class SummitSpear : ModProjectile
    {
        public int RotationTimer;
        public float size;
        public override void SetDefaults()
        {
            Projectile.width = 500;
            Projectile.height = 500;

            Projectile.timeLeft = int.MaxValue;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player powner = Main.player[Projectile.owner];
            Vector2 point = powner.RotatedRelativePoint(powner.MountedCenter + new Vector2(5 * powner.direction, -2));

            Projectile.Center = point;
            powner.heldProj = Projectile.whoAmI;

            int velX = Math.Sign(Projectile.velocity.X);

            Projectile.velocity = new Vector2(velX, 0);
            if (++Projectile.localAI[1] < 10)
                Projectile.rotation = MathHelper.ToRadians(55);
            //new Vector2(velX, powner.gravDir).ToRotation() - MathHelper.ToRadians(MathHelper.PiOver2);

            else Projectile.rotation += MathHelper.TwoPi * 4 * velX;
            Projectile.rotation -= MathHelper.ToRadians(MathHelper.Pi) * 4;
            powner.itemRotation = MathHelper.WrapAngle(Projectile.rotation);

            if (Projectile.localAI[1] > 180 && size < 1)
                size += 0.01f;

            if (!powner.channel) Projectile.Kill();
            if (++Projectile.ai[1] > 10 && Projectile.localAI[1] > 10)
            {
                SoundEngine.PlaySound(SoundID.Item169, powner.Center);
                Projectile.ai[1] = 0;
            }
        }
        public override void PostDraw(Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++)
            {

                TranscendenceUtils.DrawEntity(Projectile, Color.Lerp(Color.DarkGoldenrod, Color.White, i * 0.25f) * 0.6f, 1 + (i * 0.025f),
                    "TranscendenceMod/Miscannellous/Assets/OnePointFourSlash", Projectile.rotation + 40,
                    Projectile.Center, new Rectangle(0, 0 + (i * 172), 170, 172));

                TranscendenceUtils.DrawEntity(Projectile, Color.Lerp(Color.DarkGoldenrod, Color.White, i * 0.25f) * 0.6f, 1 + (i * 0.025f),
                    "TranscendenceMod/Miscannellous/Assets/OnePointFourSlash", Projectile.rotation - MathHelper.ToRadians(45),
                    Projectile.Center, new Rectangle(0, 0 + (i * 172), 170, 172));

            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rot = Projectile.rotation;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center
                + rot.ToRotationVector2() * 100,
                Projectile.Center + rot.ToRotationVector2() * 400) || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center
                + rot.ToRotationVector2() * -100,
                Projectile.Center + rot.ToRotationVector2() * -400))
            {
                return true;
            }
            else return false;
        }
    }
}