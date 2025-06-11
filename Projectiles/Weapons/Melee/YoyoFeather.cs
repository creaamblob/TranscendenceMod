using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class YoyoFeather : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 26;

            Projectile.penetrate = 3;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 180;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {
            Projectile projectile = Main.projectile[(int)Projectile.ai[1]];

            if (projectile == null || !projectile.active || projectile.type != ModContent.ProjectileType<SkyyoProj>())
            {
                Projectile.Kill();
                return;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.velocity = Projectile.DirectionTo(projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * Projectile.ai[2] / 6f + (TranscendenceWorld.UniversalRotation * 12f)) * 75f) * 20f;
        }
        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.Harpy, 0f, 0f, 0, default, 1f);
                Main.dust[dust].velocity = new Vector2(0, 5).RotatedBy(MathHelper.TwoPi * i / 5f);
                Main.dust[dust].noGravity = true;
            }
            return true;
        }
    }
}