using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class GodlessTendril : ModProjectile
    {
        public int Timer;
        public float Rotation;
        public float DesiredDirection;

        public float GalaxyColorFadeTimer = 520;
        public float GalaxyColorFade = 0.05f;
        public Vector2 vel;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 60;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;

            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 115;

            Projectile.friendly = true;
            Projectile.tileCollide = false;

            Projectile.localNPCHitCooldown = 20;
            Projectile.usesLocalNPCImmunity = true;

            Projectile.ignoreWater = true;
            Projectile.light = 0.5f;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            GalaxyColorFadeTimer += GalaxyColorFade;

            if (Projectile.timeLeft > 113)
            {
                vel = Projectile.velocity;
                return;
            }

            if (++Projectile.ai[1] < 35)
            {
                float multiplier = 1.2f;
                Projectile.velocity = vel.RotatedBy(Math.Sin(GalaxyColorFadeTimer * Projectile.ai[0] * multiplier) * 0.6f);
            }
            else Projectile.velocity *= 0.9f;

            if (Projectile.timeLeft < 60)
                Projectile.ai[2] += 1f / 60f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 75;

            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

            for (int i = 0; i < (Projectile.oldPos.Length / 2); i++)
            {
                Vector2 pos = Projectile.oldPos[i * 2] - Main.screenPosition;

                Main.EntitySpriteDraw(sprite, pos, null, Color.White * 0.25f * (1f - Projectile.ai[2]), Projectile.oldRot[i * 2], sprite.Size() * 0.5f, Projectile.scale * 1.5f * MathHelper.Lerp(0f, 3f - Projectile.ai[2], i / (float)Projectile.oldPos.Length), SpriteEffects.None);
                Main.EntitySpriteDraw(sprite, pos, null, Color.White * (1f - Projectile.ai[2]), Projectile.oldRot[i * 2], sprite.Size() * 0.5f, Projectile.scale * MathHelper.Lerp(0f, 3f - Projectile.ai[2], i / (float)Projectile.oldPos.Length), SpriteEffects.None);
            }
            return false;
        }
    }
}