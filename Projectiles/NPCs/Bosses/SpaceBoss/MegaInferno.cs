using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class MegaInferno : ModProjectile
    {
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 2;
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 76;
            Projectile.timeLeft = 300;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().ModUnparryable = true;
        }
        public override bool? CanDamage() => Projectile.timeLeft > 30;
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 14)
                return true;
            else
                return false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;


            if (Projectile.timeLeft < 30)
                Projectile.scale -= 1 / 30f;

            TranscendenceUtils.AnimateProj(Projectile, 3);
            Projectile.localAI[2]++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver4) * (22.5f * Projectile.scale);
            TranscendenceUtils.DrawEntity(Projectile, Color.Blue * 0.75f * Projectile.scale, 2f, "bloom", 0, pos, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Aqua * Projectile.scale, 1f, "bloom", 0, pos, null);
            return base.PreDraw(ref lightColor);
        }
    }
}