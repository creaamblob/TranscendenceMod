using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class SpaceRubble : ModProjectile
    {
        public Vector2 vel;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 26;
            Projectile.timeLeft = 600;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.rotation += 0.1f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 3);
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);

            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 16)
                return true;
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, true);

            return true;
        }
    }
}