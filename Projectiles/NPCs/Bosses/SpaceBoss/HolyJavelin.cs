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
    public class HolyJavelin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 80;

            Projectile.timeLeft = 900;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.extraUpdates = 2;
            Projectile.light = 0.75f;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center + (Projectile.rotation.ToRotationVector2() * 30f)) < 15)
                return true;
            else return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void AI()
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);


            TranscendenceUtils.DrawTrailProj(Projectile, Color.Gold, Projectile.scale, Texture, false, true, 3f, Vector2.Zero);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);


            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, false);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);


            if (Projectile.velocity == Vector2.Zero)
                Projectile.rotation = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().PreTimeStopVel.ToRotation() + MathHelper.PiOver2;

            return false;
        }
    }
}