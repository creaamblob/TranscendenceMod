using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class FrozenMawsProj : ModProjectile
    {
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 3;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 45;

            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(ModContent.BuffType<FrostBite>(), 300);
        }
        public override bool? CanDamage() => Projectile.timeLeft >= 15;
        public override void AI()
        {
            TranscendenceUtils.AnimateProj(Projectile, 7);
            Projectile.velocity = Projectile.velocity.RotatedBy(0.2f);

            // Fade out
            if (Projectile.timeLeft < 15)
                Projectile.ai[2] += 1f / 15f;

            Lighting.AddLight(Projectile.Center, new Color(0f, 0.3f, 0.5f).ToVector3());
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White * (1f - Projectile.ai[2]), Projectile.scale, Texture, 0f, Projectile.Center, false, false, false);
            return false;
        }
    }
}