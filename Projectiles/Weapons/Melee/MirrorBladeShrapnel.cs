using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class MirrorBladeShrapnel : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Main.projFrames[Type] = 3;
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
            Projectile.penetrate = 5;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
            Projectile.timeLeft = 90;

            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {
            Projectile.rotation += 0.01f;
            if (Projectile.timeLeft < 30)
                Projectile.scale -= 1f / 30f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.NextFloat(-1f, 1f);
            Projectile.frame = Main.rand.Next(0, 2);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Shatter with { MaxInstances = 0}, Projectile.Center);
            return true;
        }
    }
}