using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class ShrinkingOrb : ModProjectile
    {
        public bool AtNPC;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 520;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;

            Projectile.scale = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (10f * Projectile.scale) && Projectile.active) return true;
            else return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue, Projectile.scale * 0.7f,
                "bloom", 0, Projectile.Center, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale * 0.8f,
                    $"{Texture}", 0, Projectile.Center, null);

            return false;
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];


            if (Projectile.ai[2] != 1 && Projectile.scale > 1)
                Projectile.scale -= 0.0275f;


            if (npc.ModNPC is CelestialSeraph boss)
            {
                Vector2 targetVelocity = Projectile.DirectionTo(boss.arenaCenter + Vector2.One.RotatedByRandom(360)
                    * Main.rand.Next(100, 180)) * 6f;

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.075f);

                if (boss.arenaCenter.Distance(Projectile.Center) < 450)
                    Projectile.ai[2] = 1;

            }


            if (Projectile.ai[2] == 1)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 0.05f);

            if (Projectile.scale < 0.2f)
                Projectile.Kill();
        }
    }
}