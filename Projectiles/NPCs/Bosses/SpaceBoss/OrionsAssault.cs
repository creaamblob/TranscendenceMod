using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class OrionsAssault : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;

            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.extraUpdates = 2;
            Projectile.light = 0.25f;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }
        public override bool CanHitPlayer(Player target)
        {
            if (target.Center.Distance(Projectile.Center) < (15 * Projectile.scale))
                return true;
            else return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;

            TranscendenceUtils.DrawTrailProj(Projectile, Color.Gray * 0.7f, Projectile.scale, $"{Texture}", true, true, 5, Vector2.Zero);
            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, false);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}