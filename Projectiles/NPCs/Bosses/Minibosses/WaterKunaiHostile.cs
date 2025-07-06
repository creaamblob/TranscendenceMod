using Terraria;
using Terraria.ID;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Minibosses
{
    public class WaterKunaiHostile : WaterKunaiProj
    {
        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;

            Projectile.width = 6;
            Projectile.height = 6;

            Projectile.tileCollide = false;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
        }
        public override void AI()
        {
            int d = Dust.NewDust(Projectile.Center, 1, 1, DustID.DungeonWater);
            Main.dust[d].noGravity = true;
            Lighting.AddLight(Projectile.Center, 0, 0, 0.6f);

            if (++Timer == 20)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player player = Main.player[p];
                    if (player.Distance(Projectile.Center) < 1750)
                    {
                        Projectile.velocity = Projectile.DirectionTo(player.Center) * Chase;
                    }
                }
            }
            if (Timer > 35 && !NPC.downedPlantBoss)
                Projectile.tileCollide = true;
        }
    }
}