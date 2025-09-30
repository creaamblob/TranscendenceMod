using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class GlacierSnow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.aiStyle = -1;
            Projectile.penetrate = 5;
            Projectile.light = 0.33f; 
            Projectile.timeLeft = 45;

            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 10)
                Projectile.scale -= 1f / 10f;

            if (Projectile.ai[2] == -1)
                return;

            if (Projectile.ai[2] < 30)
                Projectile.ai[2]++;
            
            Projectile.velocity = Vector2.Lerp(Vector2.Zero, Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel.RotatedBy(MathHelper.ToRadians(Projectile.ai[2] * 4)), Projectile.ai[2] / 30f);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
                Dust.NewDust(Projectile.Center, 1, 1, DustID.Snow, 0, 5);
            return true;
        }
    }
}