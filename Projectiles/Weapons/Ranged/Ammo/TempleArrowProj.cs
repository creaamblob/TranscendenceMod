using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged.Ammo
{
    public class TempleArrowProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Ranged/Ammo/TempleArrow";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.timeLeft = 450;
            Projectile.penetrate = 1;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 12, DustID.t_Lihzahrd, Color.White, 2f, 1.25f, 2f);

            Player player = Main.player[Projectile.owner];
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, new Vector2(0, -5), ModContent.ProjectileType<TempleHead>(), Projectile.damage / 2, Projectile.knockBack * 3, player.whoAmI);

            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation()+ MathHelper.PiOver2;
            Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Vector2.Zero, 0, default, 2f);
            d.velocity = Vector2.Zero;
            d.noGravity = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, Texture + "_Glow", false, true, 2f, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}