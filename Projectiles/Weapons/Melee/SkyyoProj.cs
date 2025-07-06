using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class SkyyoProj : ModProjectile
    {
        public int Timer;
        Player player;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 10f;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = 99;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (++Timer % 45 == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, 7.5f).RotatedBy(MathHelper.TwoPi * i / 6f), ModContent.ProjectileType<YoyoFeather>(),
                        (int)(Projectile.damage * 0.66f), Projectile.knockBack, player.whoAmI, 0, Projectile.whoAmI, i);
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}