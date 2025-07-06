using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class BlossomBoom : ModProjectile
    {
        public float Rad;
        public float MaxRad = 120;
        public float Alpha = 1;
        public int Timer;
        public Color color = Color.White;
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 7;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 36;

            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.light = 1.75f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (30 * Projectile.scale))
                return true;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void AI()
        {
            Projectile.position = Projectile.Center;
            TranscendenceUtils.AnimateProj(Projectile, 5);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, Texture, 0f, Projectile.Center, false, false, false);
            return false;
        }
    }
}