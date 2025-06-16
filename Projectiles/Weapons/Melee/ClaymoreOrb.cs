using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class ClaymoreOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 26;

            Projectile.aiStyle = -1;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 150;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.timeLeft > 90)
                Projectile.scale += 6f / 60f;
            else Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center) * 24f, 0.05f);

            if (Projectile.timeLeft < 45)
                Projectile.scale -= 6f / 45f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (25 * Projectile.scale))
                return true;
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 7; i++)
                Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<NovaDust>(), Main.rand.NextFloat(-3f, 3f), 5);
            return true;
        }
    }
}