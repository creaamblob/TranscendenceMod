using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class NucleusLaser : ModProjectile
    {
        public int Timer;
        public int Timer2;
        public Vector2 pos;
        public float Speed = 0;
        public Player player;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 380;
            Projectile.hostile = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            TranscendenceUtils.AnimateProj(Projectile, 6);

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p != null && p.active && p.type == ModContent.ProjectileType<NucleusTarget>() && p.ai[0] == Projectile.ai[0] && p.Distance(Projectile.Center) < 25)
                {
                    SoundEngine.PlaySound(ModSoundstyles.SeraphBomb with { MaxInstances = 0 }, p.Center);
                    Projectile.Kill();
                    p.Kill();
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NucleusLaserBoom>(), 100, 2f, -1, 0, Projectile.ai[1]);
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 8, DustID.TheDestroyer, Color.White, 1f, 2f, 4f);
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White * 0.66f, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, false, false, false);


            return false;
        }
    }
}