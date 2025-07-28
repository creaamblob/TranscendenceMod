using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class SewerHoopProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/SewerHoop";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;

            Projectile.timeLeft = 3600;
            Projectile.aiStyle = -1;

            Projectile.penetrate = 2;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // DO NOT FLIP THE SPRITE AT ALL COSTS !!
            Projectile.spriteDirection = 1;

            Projectile.rotation += 0.5f;

            if (++Projectile.ai[2] >= 45)
                Projectile.ai[1] = 1;

            if (Projectile.ai[1] == 1)
            {
                Projectile.velocity = Projectile.DirectionTo(player.Center) * 24f;
                if (Projectile.Distance(player.Center) < 25)
                    Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (Projectile.penetrate == 1)
            {
                Projectile.ai[1] = 1;
                Projectile.penetrate = -1;
            }

            int fount = ModContent.ProjectileType<SewerHoopFountain>();

            if (Main.player[Projectile.owner].ownedProjectileCounts[fount] < 3)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, fount, Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[1] = 1;
            Projectile.tileCollide = false;

            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }
    }
}