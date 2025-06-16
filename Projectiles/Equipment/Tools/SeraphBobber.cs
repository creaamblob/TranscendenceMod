using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment.Tools
{
    public class SeraphBobber : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 18;
            Projectile.timeLeft = 120;

            Projectile.aiStyle = 61;
            Projectile.penetrate = -1;
            Projectile.bobber = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source) { }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.5f, 0.1f, 0.4f);
            //Main.NewText(Projectile.ai[0] + " " + Projectile.ai[1] + " " + Projectile.ai[2]);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] == 0)
                TranscendenceUtils.DrawEntity(Projectile, Color.Magenta, 0.7f, "bloom", 0, Projectile.Center, null);
            return true;
        }
    }
}