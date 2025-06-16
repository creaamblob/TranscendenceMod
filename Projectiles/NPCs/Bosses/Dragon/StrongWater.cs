using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class StrongWater : ModProjectile
    {
        public Player player;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override string Texture => "TranscendenceMod/Miscannellous/Assets/RainDrop";

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 53;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 450;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, lightColor * 0.75f, 1f, Texture, false, true, 0.33f, Vector2.Zero);
            TranscendenceUtils.DrawEntity(Projectile, lightColor * 0.5f, 1f, Texture, Projectile.rotation, Projectile.Center, null);
            return false;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (npc == null || !npc.active)
                return;
            //if (++Projectile.ai[2] == 15) Projectile.velocity = Projectile.DirectionTo(Main.player[npc.target].Center) * 15;
            
            //Projectile.velocity.Y += 1.5f;
            /*if (npc.ModNPC is WindDragon boss)
            {
                if (Projectile.Center.Y > boss.arenaStartPos.Y)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center,
                        Projectile.DirectionTo(Main.player[npc.target].Center) * 15, ModContent.ProjectileType<Icicle>(),
                        Projectile.damage, Projectile.knockBack, -1, 0, npc.whoAmI);
                    Projectile.Kill();
                }
            }*/
            //Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;

            //For Hell Worm boss
            /*if (++Projectile.ai[2] > 60 && Projectile.ai[2] < 120)
                Projectile.velocity *= 0.95f;

            if (player != null && player.Distance(Projectile.Center) < 2500 && Projectile.ai[2] == 180)
            {
                Vector2 targetVelocity = Projectile.DirectionTo(player.Center) * 12.5f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.85f);
            }*/

            //Fun maze attack
            /*if (player != null && player.Distance(Projectile.Center) < 250)
                Projectile.velocity = Projectile.DirectionTo(player.Center);*/
        }
        public override void OnSpawn(IEntitySource source)
        {
            /*for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player2 = Main.player[p];
                if (player2 != null && player2.active)
                {
                    player = player2;
                    //Fun maze attack
                    //if (player.Distance(Projectile.Center) < 150)
                        //Projectile.Kill();
                }
            }*/

            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 2; i++) Dust.NewDust(Projectile.Center, 8, 8, ModContent.DustType<BetterWater>());
            SoundEngine.PlaySound(SoundID.Drip);
        }
    }
}