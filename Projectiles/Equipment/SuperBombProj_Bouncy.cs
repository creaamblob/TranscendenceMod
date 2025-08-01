using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class SuperBombProj_Bouncy : ModProjectile
    {
        public Vector2 vel;
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.aiStyle = -1;

            Projectile.ignoreWater = true;
            Projectile.timeLeft = 140;
            Projectile.tileCollide = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.Lerp(new Color(1f, 0.8f, 0f, 0f), new Color(1f, 0.1f, 0f, 0f), (float)Math.Sin(Main.GlobalTimeWrappedHourly * 8)) * 0.125f, 1.8f, "TranscendenceMod/Miscannellous/Assets/GradientCircle", Projectile.rotation, Projectile.Center, null);
            return base.PreDraw(ref lightColor);
        }
        public override void PostDraw(Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, "TranscendenceMod/Projectiles/Equipment/SuperBombProj_Glow", Projectile.rotation, Projectile.Center, null);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.Red * 2, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 8));
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = -oldVelocity;
            return false;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length() / 2f);
            Projectile.velocity.Y += 0.175f;

            if (Projectile.timeLeft < 20)
            {
                Projectile.scale += 1f / 20f;
                return;
            }
        }
        public override bool PreKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SuperBoom>(), 200, Projectile.knockBack, Projectile.owner);
            return base.PreKill(timeLeft);
        }
    }
}