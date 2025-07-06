using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class Asteroid : ModProjectile
    {
        public static int AITimer;
        public static int Direction;
        public static int DistanceMultiplier = 65;
        public static float Speed = 1.0075f;
        NPC npc;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 900;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }
        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }
        public override void OnSpawn(IEntitySource source) => TranscendenceUtils.NoExpertProjDamage(Projectile);
        public override void AI()
        {
            npc = Main.npc[(int)Projectile.ai[1]];
            Projectile.velocity = Projectile.DirectionTo(npc.Center) * Projectile.ai[2];
            Projectile.rotation += MathHelper.ToRadians(5);

            if (Projectile.ai[0] < 1f)
                Projectile.ai[0] += 1f / 30f;

            if (Projectile.Distance(npc.Center) < 175)
                Projectile.Kill();
        }
        public override void PostDraw(Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.White * Projectile.ai[0], Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, null);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //TranscendenceUtils.DrawEntityOutlines(Projectile, TranscendenceWorld.BlackholeColor, Projectile.scale * 1.2f, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}