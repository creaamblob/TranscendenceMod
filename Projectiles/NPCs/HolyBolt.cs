using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs
{
    public class HolyBolt : ModProjectile
    {
        public int Timer;
        public float ChaseSpeed = 12.5f;
        public Vector2 pos;
        private Texture2D sprite2;
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void PostDraw(Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.Gold * 0.66f, 1, "bloom", 0, Projectile.Center, null);
        }
        public override void AI()
        {
            Timer++;
            if (Timer < (15 + Projectile.ai[2])) Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];

                if (player != null && player.active && player.Distance(Projectile.Center) < 2750)
                {
                    if (Timer < (15 + Projectile.ai[2]))
                        pos = player.Center;

                    if (Timer < (15 + Projectile.ai[2]) || Timer > (25 + Projectile.ai[2]))
                        return;
                    Vector2 vel2 = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().startPos.DirectionTo(player.Center);
                    Projectile.velocity = vel2 * ChaseSpeed;
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
    }
}