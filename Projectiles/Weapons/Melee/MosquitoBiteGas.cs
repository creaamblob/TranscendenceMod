using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class MosquitoBiteGas : ModProjectile
    {
        public override void SetStaticDefaults() => Main.projFrames[Type] = 4;
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 28;

            Projectile.timeLeft = 160;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 5;
            Projectile.ArmorPenetration = 50;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;

            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            TranscendenceUtils.AnimateProj(Projectile, 5);
            NPC target = Projectile.FindTargetWithinRange(250);
            if (target != null && Projectile.ai[0] == 1 && ++Projectile.ai[2] > 10)
            {
                Vector2 targetVelocity = Projectile.DirectionTo(target.Center + Vector2.One.RotatedByRandom(360) * Main.rand.Next(20, 130)) * 9;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.075f);
            }
            Projectile.ai[1]++;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != Projectile.oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != Projectile.oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 4; i++)
            {
                float pi = MathHelper.PiOver2 * i / 4;
                Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * pi)) * 15;
                TranscendenceUtils.DrawProjAnimated(Projectile, lightColor * 0.85f, Projectile.scale * 1.25f, $"{Texture}", 0, pos, false, false, false);
            }
            TranscendenceUtils.DrawEntity(Projectile, Color.LimeGreen * 0.75f, 1.25f, "bloom", 0, Projectile.Center, null);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            TranscendenceUtils.DrawProjAnimated(Projectile, lightColor, Projectile.scale, $"{Texture}", 0, Projectile.Center, false, false, false);
        }
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 10, DustID.Poisoned, Main.rand.Next(-3, 3),
                Color.White, Main.rand.NextFloat(0.3f, 1.4f));
            return base.PreKill(timeLeft);
        }
    }
}