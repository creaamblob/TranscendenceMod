using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class BloodFireball : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 450;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity.Y += 0.1f;
        }
    }
}