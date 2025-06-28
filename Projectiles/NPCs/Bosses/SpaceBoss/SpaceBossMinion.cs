using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class SpaceBossMinion : ModProjectile
    {
        public int CD;
        bool Charged;
        Player player;
        public Vector2 pos;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.width = 32;
            Projectile.height = 34;
            Projectile.timeLeft = 360;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            CD = 20;
            Charged = false;
            TranscendenceUtils.NoExpertProjDamage(Projectile);

            SoundEngine.PlaySound(SoundID.AbigailUpgrade with
            {
                MaxInstances = 0,
                Volume = 0.45f
            }, Projectile.Center);
        }
        public override void AI()
        {
            int proj = ModContent.ProjectileType<MegaInferno>();

            if (Main.npc[(int)Projectile.ai[1]].ModNPC is CelestialSeraph boss)
            {

                Vector2 yvel = new Vector2(0, 5f);
                Projectile.spriteDirection = Projectile.direction;

                if (Main.rand.NextBool(10)) Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<ArenaDust>(),
                    -Projectile.velocity.X, -Projectile.velocity.Y, 0, Color.BlueViolet, 0.45f + (Projectile.scale * 0.65f));

                if (++Projectile.localAI[2] > 60)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        player = Main.player[i];
                        if (player != null && player.active && player.Distance(Projectile.Center) < 1200 && (Main.expertMode || Main.masterMode) && !Charged && (boss.Phase > 1 || boss.Timer_AI > 320) && Projectile.localAI[2] < 65 && boss.Phase > 1)
                        {
                            pos = player.Center;
                            Projectile.velocity = Projectile.DirectionTo(pos) * 20;
                            Charged = true;
                        }
                    }
                }
                else
                {
                    if (++CD > Projectile.ai[2])
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int a = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, yvel,
                                proj, Projectile.damage * ((Main.masterMode || Main.expertMode) ? 2 : 1), 1f, -1, 0, Projectile.ai[1], 0);

                            Projectile pa = Main.projectile[a];
                            pa.velocity.X = boss.Phase == 2 ? 2f : 1f;
                            pa.extraUpdates = 2;

                            int b = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, yvel,
                            proj, Projectile.damage * ((Main.masterMode || Main.expertMode) ? 2 : 1), 1f, -1, 0, Projectile.ai[1], -1);

                            Projectile pb = Main.projectile[b];
                            pb.velocity.X = boss.Phase == 2 ? -2f : -1f;
                            pb.extraUpdates = 2;
                        }
                        CD = 0;
                    }
                }
            }
            /*if (Projectile.localAI[2] < 55 && Projectile.localAI[2] > 20 && ++Projectile.localAI[1] > 5)
            {
                for (int i = 0; i < 7; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(0, 10), new Vector2(0, 3 + i),
                   ModContent.ProjectileType<DashBallProj2>(), Projectile.damage, 1f, 0, 5, Main.npc[(int)Projectile.ai[1]].whoAmI, 0.5f);
                }
                Projectile.localAI[1] = 0;
            }*/
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.AnimateProj(Projectile, 10);

            SpriteBatch sb = Main.spriteBatch;

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.BlueViolet * 0.5f, 2f,
                "TranscendenceMod/Miscannellous/Assets/StarEffect", true, false, 1, Vector2.Zero);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.Magenta * 0.5f, 1f,
    "TranscendenceMod/Miscannellous/Assets/StarEffect", true, false, 1, Vector2.Zero);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, true);
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, false, false, false);

            return false;
        }
    }
}