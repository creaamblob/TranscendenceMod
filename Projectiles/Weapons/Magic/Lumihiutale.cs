using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class Lumihiutale : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;

            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 1;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void OnSpawn(IEntitySource source) { Projectile.ai[1] = Main.rand.Next(-1, 2); }
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DeerclopsIceAttack with { MaxInstances = 0}, Projectile.Center);
            for (int t = 0; t < 8; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SnowstormDust>(), new Vector2(0, 5).RotatedBy(MathHelper.TwoPi * t / 8f), 0, Color.White, 1.25f);
                dust.noGravity = true;
            }
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation += 0.025f;

            NPC npc = Projectile.FindTargetWithinRange(500, true);
            Player player = Main.player[Projectile.owner];

            if (player == null || !player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            if (npc != null && npc.active && Projectile.ai[2] == 0 && npc.Distance(Projectile.Center) < (npc.width * 1.25f) && npc.Distance(Projectile.Center) < (npc.height * 1.25f))
            {
                float rot = (player.Center - npc.Center).ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4;
                Vector2 pos = npc.Center + Vector2.One.RotatedBy(rot) * 250;

                SpawnDust();

                Projectile.Center = pos;
                Projectile.velocity = Projectile.DirectionTo(player.Center).RotatedByRandom(0.125f) * 15;

                SpawnDust();

                Projectile.ai[2] = 1;
            }
            else if (Projectile.ai[2] != 1) Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(0f, 0.0125f) * Projectile.ai[1]) * 0.975f;
            void SpawnDust()
            {
                for (int t = 0; t < 8; t++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.FrostStaff, new Vector2(0, 5).RotatedBy(MathHelper.TwoPi * t / 8), 0, Color.White, 2f);
                    dust.noGravity = true;
                }
            }
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.BetterDrawTrailProj(Projectile, Color.White * 0.35f, Projectile.scale, $"{Texture}", 0.25f, true, 1f, Vector2.Zero, 0);
            return base.PreDraw(ref lightColor);
        }
    }
}