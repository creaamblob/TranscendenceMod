using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class DragonBolt_Telegraph : ModProjectile
    {
        public float startVel;
        public int Timer;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.extraUpdates = 2;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 450;
        }
        public override void AI()
        {
            base.AI();
            //Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<ArenaDust>(), Vector2.Zero, 0, Color.DeepSkyBlue, 0.75f);

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
            ProjectileID.Sets.TrailCacheLength[Type] = 80;
            ProjectileID.Sets.TrailingMode[Type] = 3;

            TranscendenceUtils.DrawTrailProj(Projectile, Color.DeepSkyBlue, 5, "TranscendenceMod/Miscannellous/Assets/Pixel", false, false, 1, Vector2.Zero);

            return base.PreDraw(ref lightColor);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
                Dust.NewDust(Projectile.Center, 8, 8, DustID.Electric);
        }
    }
}