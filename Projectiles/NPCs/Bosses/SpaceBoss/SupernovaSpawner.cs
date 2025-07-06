using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class SupernovaSpawner : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public int Timer;
        public int Timer2;
        public int ProjTimer;
        public float Rotation;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 200;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
            Projectile.hostile = true;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.light = 0.1f;
        }
        public override bool CanHitPlayer(Player target) => false;
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public bool SuitableForSpawn()
        {
            return false;
        }
        public override void AI()
        {
            int d = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<ArenaDust>(), 0, 0, 0, Color.White, 1.25f);
            Main.dust[d].velocity = Vector2.Zero;

            if (++Timer2 < 70) Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (90 * Projectile.ai[2])) * 1.01f;

            if (Timer2 == 110)
            {
                //Projectile.ai[2] = -Projectile.ai[2];
                SoundEngine.PlaySound(SoundID.AbigailUpgrade with
                {
                    MaxInstances = 0,
                    Volume = 0.95f
                }, Projectile.Center);
            }
            if (++ProjTimer % Projectile.ai[1] == 0 && Timer2 < 125)
            {
                int p2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<SpaceBossBomb>(), Projectile.damage, 0, -1);
                Main.projectile[p2].localAI[1] = Projectile.ai[0];
            }
        }
    }
}