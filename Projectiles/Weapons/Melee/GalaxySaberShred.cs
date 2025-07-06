using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class GalaxySaberShred : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public int HomeTimer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 55;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.timeLeft = 35;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 10;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            NPC target = Projectile.FindTargetWithinRange(500);
            HomeTimer++;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (target != null && HomeTimer > 5 && HomeTimer < 50)
            {
                Vector2 targetVelocity = Projectile.DirectionTo(target.Center + Vector2.One.RotatedByRandom(360)
                    * Main.rand.Next(10, 50)) * 42;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.075f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Projectile.ai[1] == 1 ? Color.DarkMagenta :
                Projectile.ai[1] == 2 ? Color.DarkBlue : Color.BlueViolet, 1.75f,
                "TranscendenceMod/Miscannellous/Assets/Trail", true, false, 1, Vector2.Zero);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 1f, "TranscendenceMod/Miscannellous/Assets/Trail", true, false, 1, Vector2.Zero);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}