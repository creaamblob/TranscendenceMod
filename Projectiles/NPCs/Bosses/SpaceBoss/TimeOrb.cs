using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class TimeOrb : ModProjectile
    {
        public float MinuteRot;
        public float HourRot;
        public Vector2 vel;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.timeLeft = 360;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            vel = Projectile.velocity;
        }
        public override void AI()
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.rotation -= 0.1f;
            HourRot = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().PreTimeStopVel.ToRotation();
            MinuteRot += 0.2133f;

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, false);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, $"{Texture}_Arm", MinuteRot, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, $"{Texture}_Arm2", HourRot, Projectile.Center, null);
            return false;
        }
    }
}