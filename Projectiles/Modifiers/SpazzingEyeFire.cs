using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Modifiers
{
    public class SpazzingEyeFire : ModProjectile
    {
        Rectangle rec;
        public float Opacity = 1;
        public float Rotation;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 6;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 18;

            Projectile.ownerHitCheck = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.projFrames[Type] = 7;

            Rotation += 0.15f;
            for (int i = 1; i < 7; i++)
            {
                TranscendenceUtils.DrawProjAnimated(Projectile, Color.Lime, i * 0.1f * Main.rand.NextFloat(0.75f, 1.25f), "TranscendenceMod/Projectiles/Equipment/FireworkBlast",
                    Rotation, Projectile.Center + Projectile.velocity * (i * 5), false, false, false);

                TranscendenceUtils.DrawEntity(Projectile, Color.LimeGreen * 0.6f * (i == 1 ? 0 : 1), 0.75f + (i * 0.3f), "bloom", 0, Projectile.Center + Projectile.velocity * (i * 6), null);

                TranscendenceUtils.DrawProjAnimated(Projectile, Color.Yellow, i * 0.05f * Main.rand.NextFloat(0.75f, 1.25f), "TranscendenceMod/Projectiles/Equipment/FireworkBlast",
                    Rotation, Projectile.Center + Projectile.velocity * (i * 5), false, false, false);
            }
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            TranscendenceUtils.AnimateProj(Projectile, 8);
            
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * 30, 10, ref reference))
            {
                return true;
            }
            else return false;
        }
    }
}