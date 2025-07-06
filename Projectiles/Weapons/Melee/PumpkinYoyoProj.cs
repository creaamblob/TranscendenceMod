using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class PumpkinYoyoProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 15;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 180;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 18;
        }

        public override void SetDefaults()
        {
            Projectile.width = 19;
            Projectile.height = 18;

            Projectile.aiStyle = 99;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 6;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity = Projectile.DirectionTo(target.Center) * 6;
            SoundEngine.PlaySound(SoundID.NPCDeath21, Projectile.position);
            for (int i = 0; i < 7; i++)
            {
                Vector2 bsagfs = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Pumpkin>(), bsagfs);
            }
        }
    }
}