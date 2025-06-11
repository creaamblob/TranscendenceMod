using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged.Ammo
{
    public class TwentyTwoBulletProj : ModProjectile
    {
        public List<NPC> hitNpcs = new List<NPC>();
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 6;
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.ai[2];

        //Just in case
        public override void OnSpawn(IEntitySource source) => hitNpcs.Clear();

        public override void AI()
        {
            if (Projectile.ai[2] < 1f)
                Projectile.ai[2] += 1f / 20f;

            if (hitNpcs.Count > 21)
                Projectile.Kill();

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Lighting.AddLight(Projectile.Center, 0.4f, 0.3f, 0f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n != null && n.active && n.Distance(Projectile.Center) < 1500f && n != target && n.chaseable && !hitNpcs.Contains(n))
                {
                    Projectile.velocity = Projectile.DirectionTo(n.Center) * 11f;

                    for (int j = 0; j < 12; j++)
                    {
                        Dust.NewDustPerfect(target.Center, DustID.GoldCoin, target.DirectionTo(n.Center).RotatedByRandom(0.2f) * Main.rand.NextFloat(3f, 6f), 0, default, 0.5f);
                    }

                    if (!hitNpcs.Contains(target))
                        hitNpcs.Add(target);

                    break;
                }
            }

            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 10, DustID.GoldCoin, Color.White, 1f, 0f, 4f);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
    }
}