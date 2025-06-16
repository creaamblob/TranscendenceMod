using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class WindBlastHostile : ModProjectile
    {
        public int Timer;
        public int Timer2;
        public float Speed = 0;
        public override string Texture => "TranscendenceMod/Projectiles/Equipment/WindBlast";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = false;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 240;
            Projectile.hostile = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Timer2 == 95 && Projectile.ai[2] != 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2Circular(8, 8),
                        Type, Projectile.damage * (Main.expertMode || Main.masterMode ? 2 : 1), Projectile.knockBack,
                        -1, 0, Main.npc[(int)Projectile.ai[1]].whoAmI, Projectile.ai[2] + 1);
                }
            }

            if (Speed < 45)
                Speed += 0.75f;

            Timer2++;
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];

                if (player.Distance(Projectile.Center) < 2500 && Timer2 > 10 && ++Timer > Main.rand.Next(6, 8) && Timer2 < 60)
                {
                    Vector2 targetVelocity = Projectile.DirectionTo(player.Center + Vector2.One.RotatedByRandom(360) * Main.rand.Next(10, 75)) * Speed;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.33f);
                    //(player.Center + player.velocity * 4 - Projectile.Center).SafeNormalize(Vector2.Zero) * ChaseSpeed * ChaseSpeed / 100f
                    //Projectile.velocity += Vector2.Normalize(player.Center - Projectile.Center) * ChaseSpeed / 15;
                    Timer = 0;
                }
            }

            /*if (++Timer > 3 && Projectile.ai[2] != 0)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (240 * Projectile.ai[2]));
                Timer = 0;
            }*/
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Transparent;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void PostDraw(Color lightColor)
        {
            Color col = Color.White;
            TranscendenceUtils.AnimateProj(Projectile, 3);
            TranscendenceUtils.DrawProjAnimated(Projectile, col, 1, $"{Texture}", Projectile.rotation, Vector2.Zero, true, true, true);
        }
    }
}