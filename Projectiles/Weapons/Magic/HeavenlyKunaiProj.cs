using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class HeavenlyKunaiProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.timeLeft = 45;
            Projectile.penetrate = 2;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.ai[2];

        public override void OnSpawn(IEntitySource source) => Projectile.ai[2] = 1f;
        public override void AI()
        {
            if (Projectile.timeLeft < 38)
            {
                Projectile.velocity *= 0.9f;
                Projectile.ai[2] -= 1f / 30f;
            }
            else Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 8, ModContent.DustType<HolyDust>(), Color.White, 1f, 2f, 4f);
            return base.PreKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            return base.PreDraw(ref lightColor);
        }
    }
}