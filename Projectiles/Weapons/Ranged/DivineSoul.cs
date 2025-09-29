using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class DivineSoul : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 34;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void OnSpawn(IEntitySource source) => SoundEngine.PlaySound(SoundID.NPCDeath52);
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath52 with { MaxInstances = 0}, Projectile.Center);
            for (int t = 0; t < 20; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.DungeonSpirit, Main.rand.NextVector2CircularEdge(1f, 1f) * 5, 0, Color.White, 1.55f);
                dust.noGravity = true;

            }
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            TranscendenceUtils.AnimateProj(Projectile, 4);

            int range = 100;
            if (Projectile.ai[1] == 5) range = 500;

            NPC npc = Projectile.FindTargetWithinRange(range, true);
            if (npc != null) Projectile.velocity = Projectile.DirectionTo(new Vector2(npc.Center.X + Main.rand.Next(-50, 50), npc.Center.Y + Main.rand.Next(-50, 50))) * 9;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            /*if (Projectile.ai[1] == 5)
            {
                TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, $"{Texture}_Powered", Projectile.rotation, -Projectile.Size, true, false, true);
                TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, $"{Texture}_Powered", Projectile.rotation, Projectile.Center, false, false, false);
                return false;
            }*/
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White * 0.75f, Projectile.scale, $"{Texture}", 0, Vector2.Zero, true, false, true);
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, false, false, false);
            return false;
        }
    }
} 