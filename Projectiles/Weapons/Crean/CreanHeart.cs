using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Crean
{
    public class CreanHeart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 33;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 240;
            Projectile.scale = 1f;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.extraUpdates = 3;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 vel = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel;

            if (Projectile.localAI[1] > 0)
                Projectile.localAI[1]--;

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            //Smaller = Bigger curve
            float multiplier = 0.075f;
            Projectile.velocity = vel.RotatedBy(Math.Sin(++Projectile.ai[0] / 2f * Projectile.ai[2] * multiplier));

            if (Projectile.timeLeft < 30)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 30f);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => targetHitbox.Distance(Projectile.Center) < (20 * Projectile.scale);
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.75f, Projectile.scale * 1.1f, Texture, Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.75f, Projectile.scale * 0.9f, Texture, Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Wheat, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            return false;
        }
    }
}