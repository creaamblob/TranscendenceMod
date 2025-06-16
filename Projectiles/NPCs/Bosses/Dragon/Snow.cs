using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class Snow : ModProjectile
    {
        public float rot;
        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;

            Projectile.aiStyle = -1;

            Projectile.hostile = true;
            Projectile.timeLeft = 450;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0, 0.25f, 0.4f);
            if (++Projectile.ai[0] > 6)
            {
                if (Main.rand.NextBool(6))
                {
                    int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.Snow);
                    Main.dust[dust].noGravity = true;
                }
                if (Projectile.ai[2] != 0) Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (90 * Projectile.ai[2])) * 1.025f;
                else Projectile.velocity *= 1.025f;
                Projectile.ai[0] = 0;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            rot = Main.rand.NextFloat(-1f, 1f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            rot += 0.033f;
            TranscendenceUtils.DrawEntity(Projectile, lightColor, Projectile.scale, Texture, rot, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture + "_Glow", rot, Projectile.Center, null);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++) Dust.NewDust(Projectile.Center, 8, 8, DustID.Ice);
            SoundEngine.PlaySound(SoundID.Item48);
        }
    }
}