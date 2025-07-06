using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment.Tools
{
    public class SunwarePickaxeGuidingLight : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 120;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source) { }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.6f, 0.2f);

            Projectile.velocity *= 0.95f;
            if (Projectile.timeLeft < 25) Projectile.scale -= 0.04f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.OrangeRed, Projectile.scale, "bloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Yellow, 0.75f * Projectile.scale, "bloom", 0, Projectile.Center, null);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            //TranscendenceUtils.ParticleOrchestra(Terraria.GameContent.Drawing.ParticleOrchestraType.Excalibur, Projectile.Center, -1);
        }
    }
}