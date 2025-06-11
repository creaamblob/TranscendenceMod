using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class Human : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 24;
            Projectile.height = 46;
            Projectile.timeLeft = 630;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 0.75f, 0.75f, 1f, 0.66f, false);
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(0.0075f * Projectile.ai[2]);
        }
    }
}