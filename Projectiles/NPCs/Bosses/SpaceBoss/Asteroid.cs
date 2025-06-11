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
            for (int i = 0; i < 3; i++) Dust.NewDust(Projectile.Center, 32, 32, DustID.Wraith, Main.rand.Next(-1, 1), Main.rand.Next(-1, 1));
            return base.PreKill(timeLeft);
        }
        public override void OnSpawn(IEntitySource source) => TranscendenceUtils.NoExpertProjDamage(Projectile);
        public override void AI()
        {
            npc = Main.npc[(int)Projectile.ai[1]];
            Projectile.velocity = Projectile.DirectionTo(npc.Center) * Projectile.ai[2];
            Projectile.rotation += MathHelper.ToRadians(5);

            if (Projectile.Distance(npc.Center) < 75)
                Projectile.Kill();
        }
        public override void PostDraw(Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, lightColor * 0.8f, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, null);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //TranscendenceUtils.DrawEntityOutlines(Projectile, TranscendenceWorld.BlackholeColor, Projectile.scale * 1.2f, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}