using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class RuneBlastFriendly : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.penetrate = 4;

            Projectile.aiStyle = -1;

            Projectile.light = 0.25f;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 450;
        }
        public override void AI()
        {
            if (Projectile.timeLeft > 447)
                return;

            if (Projectile.timeLeft < 430)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy((float)Math.Tan(TranscendenceWorld.UniversalRotation) / 3f);
            }

            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.RuneWizard, 0, 0, 0, default, 0.75f);
                Main.dust[dust].noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TranscendenceUtils.DustRing(target.Center, 15, DustID.RuneWizard, 2, Color.White, 2);
            target.AddBuff(BuffID.OnFire3, 120);
        }
        public override bool PreKill(int timeLeft)
        {
            int dust = Dust.NewDust(Projectile.Center, 8, 8, DustID.Shadowflame, 0.1f, 0.1f, 0, default, 1f);
            Main.dust[dust].noGravity = true;
            return true;
        }
    }
}