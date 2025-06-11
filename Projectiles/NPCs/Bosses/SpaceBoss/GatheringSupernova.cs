using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class GatheringSupernova : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/SpaceBossMinion";
        public int Timer;
        public int Timer2;
        public int ProjTimer;
        public float Rotation;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 600;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
            Projectile.hostile = true;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.light = 0.25f;
        }
        public override bool CanHitPlayer(Player target) => false;
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;

            if (++Timer2 < 70) Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (60 * Projectile.ai[2])) * 1.01f;
            NPC npc = Main.npc[(int)Projectile.ai[1]];
            Player player = Main.player[npc.target];

            if (npc == null || player == null)
                return;

            if (Timer2 > 130 && Timer2 < 180)
            {
                Vector2 pos = npc.Center;
                Projectile.velocity = Projectile.DirectionTo(pos) * 25;
                if (Projectile.Distance(pos) < 700)
                    Projectile.velocity *= 0.5f;
            }
            Vector2 vel = npc.DirectionTo(Projectile.Center + Vector2.One.RotatedBy(Projectile.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver4) * 2000);
            /*if (Timer2 > 240 && Projectile.localAI[0] != 1 && (Main.expertMode || Main.masterMode))
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel * (4 + i), ModContent.ProjectileType<CosmicSphere>(), Projectile.damage, 0, -1);
                }
                Projectile.Kill();
            }*/
            if (Timer2 == 240)
            {
                Projectile.velocity = vel * 30f;
            }

            if (Timer2 == 180)
            {
                //Projectile.ai[2] = -Projectile.ai[2];
                SoundEngine.PlaySound(SoundID.AbigailUpgrade with
                {
                    MaxInstances = 0,
                    Volume = 0.95f
                }, Projectile.Center);
            }

            if (++ProjTimer > 11)
            {
                int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<SpaceBossBomb>(), Projectile.damage, 0, -1);
                Main.projectile[p].scale = Projectile.ai[0] / (Timer > 240 ? 4f : 1f);
                Main.projectile[p].localAI[1] = Projectile.localAI[1];
                ProjTimer = 0;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.AnimateProj(Projectile, 10);

            SpriteBatch sb = Main.spriteBatch;

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.BlueViolet * 0.5f, 1,
                "TranscendenceMod/Miscannellous/Assets/StarEffect", true, false, 1, Vector2.Zero);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.Magenta * 0.5f, 0.75f,
    "TranscendenceMod/Miscannellous/Assets/StarEffect", true, false, 1, Vector2.Zero);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return base.PreDraw(ref lightColor);
        }
    }
}