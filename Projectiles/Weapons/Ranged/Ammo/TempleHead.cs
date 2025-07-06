using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged.Ammo
{
    public class TempleHead : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 24;

            Projectile.timeLeft = 90;
            Projectile.penetrate = 2;

            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 12, DustID.t_Lihzahrd, Color.White, 2f, 1.25f, 2f);
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.velocity *= 0.95f;

            if (++Projectile.ai[2] % 30 == 0)
            {
                Projectile.velocity = Main.rand.NextVector2Circular(14f, 14f);

                int p = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld) * 12.5f, ProjectileID.EyeBeam, Projectile.damage, Projectile.knockBack * 3, player.whoAmI);
                Main.projectile[p].friendly = true;
                Main.projectile[p].DamageType = DamageClass.Ranged;
                Main.projectile[p].hostile = false;
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, Texture + "_Glow", true, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}