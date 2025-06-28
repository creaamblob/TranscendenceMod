using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class HeavyKnifeProj : ModProjectile
    {
        public Vector2 startVel;
        public NPC npc;
        public int Element;
        public override string Texture => "TranscendenceMod/Items/Weapons/Ranged/HeavyKnife";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 44;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.timeLeft = 450;
            Projectile.ArmorPenetration = 10;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            if (npc != null)
            {
                Projectile.Center = npc.Center + startVel;
                if (!npc.active)
                    Projectile.Kill();
            }
            else Projectile.velocity.Y += 0.15f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 8)
                return true;
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            startVel = Projectile.Center - target.Center;
            npc = target;
            Projectile.velocity = Projectile.DirectionTo(npc.Center) * 0.0000001f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (npc == null)
            {
                TranscendenceUtils.DrawTrailProj(Projectile, lightColor, 1, $"{Texture}", false, true, 1, Vector2.Zero);
            }
            return base.PreDraw(ref lightColor);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++) Dust.NewDust(Projectile.Center, 4, 4, DustID.Lead);
            SoundEngine.PlaySound(SoundID.Item37 with { MaxInstances = 0, Volume = 0.5f}, Projectile.Center);
        }
    }
}