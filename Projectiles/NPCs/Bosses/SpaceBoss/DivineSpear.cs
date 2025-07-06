using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class DivineSpear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;

            Projectile.timeLeft = 60;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.extraUpdates = 2;
            Projectile.light = 0.75f;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override bool PreKill(int timeLeft)
        {
            int dmg = Projectile.damage;

            if (Main.expertMode || Main.masterMode)
                dmg *= 2;

            int amount = 8;
            float scale = 12f;

            bool big = Projectile.localAI[2] == 1f;
            if (big)
                scale = 18f;

            TranscendenceUtils.ProjectileRing(Projectile, amount, Projectile.GetSource_Death(), Projectile.Center, ModContent.ProjectileType<GenericDivineLaser>(),
                dmg, 0f, 1f, 0f, Projectile.ai[1], scale, -1, Main.rand.NextFloat(MathHelper.TwoPi));

            SoundEngine.PlaySound(ModSoundstyles.SeraphSwords_Charge with { Volume = 3, PitchVariance = 0.25f });

            return base.PreKill(timeLeft);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 20)
                return true;
            else return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void AI()
        {
            if (++Projectile.ai[2] % 10 == 0 && Projectile.localAI[2] == 0f)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver2) * 4f, ModContent.ProjectileType<HolyLaserSpawner>(),
                    Projectile.damage, Projectile.knockBack, -1, 0, Projectile.ai[1]);

                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(-MathHelper.PiOver2) * 4f, ModContent.ProjectileType<HolyLaserSpawner>(),
                    Projectile.damage, Projectile.knockBack, -1, 0, Projectile.ai[1]);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.Gold * 0.66f, Projectile.scale, "TranscendenceMod/Miscannellous/Assets/Trail", false, true, 2f, Vector2.Zero);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale / 2f, "TranscendenceMod/Miscannellous/Assets/Trail", false, true, 2f, Vector2.Zero);

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, false);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}