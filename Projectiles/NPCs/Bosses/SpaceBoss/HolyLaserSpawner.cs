using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class HolyLaserSpawner : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 24;
            Projectile.timeLeft = 120;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override bool? CanDamage() => false;
        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            Projectile.rotation += 0.05f * Projectile.velocity.Length();

            if (Projectile.timeLeft == 90)
            {
                int dmg = Projectile.damage;

                if (Main.expertMode || Main.masterMode)
                    dmg *= 2;

                dmg = (int)(dmg * 0.66f);

                TranscendenceUtils.ProjectileRing(Projectile, 4, Projectile.GetSource_Death(), Projectile.Center, ModContent.ProjectileType<GenericDivineLaser>(),
                    dmg, 0f, 1f, 0, Projectile.ai[1], 2f, -1, Main.rand.NextFloat(MathHelper.TwoPi));
            }
        }
    }
}