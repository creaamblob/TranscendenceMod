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
    public class LumimyrskyHiutale : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.penetrate = 6;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void OnSpawn(IEntitySource source) { }
        public void SpawnDust(int amount)
        {
            for (int t = 0; t < amount; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<SnowstormDust>(), Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(3f, 8f), 0, Color.White, 1.25f);
                dust.noGravity = true;
            }
        }
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig with { MaxInstances = 0}, Projectile.Center);
            SpawnDust(15);
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;

            if (Main.mouseRight && Projectile.ai[2] != 1)
            {
                SpawnDust(10);
                Projectile.ai[2] = 1;
            }

            NPC npc = Projectile.FindTargetWithinRange(500, true);
            if (npc != null && npc.active && npc.Distance(Projectile.Center) < 2500 && Projectile.ai[2] != 1) Projectile.velocity = Projectile.DirectionTo(npc.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.timeLeft * 4f)) * (Projectile.timeLeft / 2)) * 15;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailingMode[Type] = 3;
            TranscendenceUtils.DrawTrailProj(Projectile, Color.Gray * 0.66f, Projectile.scale, $"{Texture}", true, true, 1f, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}