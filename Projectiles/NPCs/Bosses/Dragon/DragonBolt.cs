using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class DragonBolt : ModProjectile
    {
        public float startVel;
        public int Timer;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 450;
        }
        public override void AI()
        {
            base.AI();
            if (Projectile.timeLeft < 390)
                Projectile.extraUpdates = 1;

            if (++Timer > 15 && Timer < 30)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 15 * Projectile.ai[2];
            }
            if (Timer == 35)
            {
                Projectile.velocity.X = startVel;
                Projectile.velocity.Y = 0;
            }
            if (Timer > 45 && Timer < 60)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = -10 * Projectile.ai[2];
            }
            if (Timer == 60)
            {
                Projectile.velocity.X = startVel;
                Projectile.velocity.Y = 0;
                Timer = 0;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            startVel = Projectile.velocity.X;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, Texture, true, true, 2, -new Vector2(7, 13));
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
                Dust.NewDust(Projectile.Center, 8, 8, DustID.Electric);
        }
    }
}