using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class CobaltWater : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 100;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;

            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
        }
        public override bool PreKill(int timeLeft)
        {
            Dust.NewDust(Projectile.Center, 1, 1, DustID.DungeonWater, 0, 3, 0, Color.White, 2f);
            return base.PreKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Wet, 90);
        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.Wet, 90);
        public override void AI()
        {
            Projectile.velocity.Y += 0.66f;
            Lighting.AddLight(Projectile.Center, 0, 0.2f, 0.8f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            int d = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 15), 1, 1, ModContent.DustType<LandsiteDroplet>(),
                0, 0, 0, Color.White, 2f);
            Main.dust[d].noGravity = true;
            Main.dust[d].velocity = Vector2.Zero;

            int d2 = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2 - MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 15), 1, 1, ModContent.DustType<LandsiteDroplet>(),
                0, 0, 0, Color.White, 2f);
            Main.dust[d2].noGravity = true;
            Main.dust[d2].velocity = Vector2.Zero;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.DeepSkyBlue, 1f, "TranscendenceMod/Projectiles/Weapons/Ranged/TurmoilMiniProj", true, false, 1f ,Vector2.Zero);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return base.PreDraw(ref lightColor);
        }
    }
}