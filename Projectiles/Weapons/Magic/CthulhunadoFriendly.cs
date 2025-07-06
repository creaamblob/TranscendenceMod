using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class CthulhunadoFriendly : ModProjectile
    {
        Rectangle rec;
        public float Opacity = 1;
        public float Timer;
        public float Timer2;
        public int proj = ModContent.ProjectileType<PoCStar>();
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
            Projectile.timeLeft = 15;

            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.AnimateProj(Projectile, 7);
            for (int i = 0; i < 16; i++)
            {
                TranscendenceUtils.DrawProjAnimated(Projectile, Color.White * Opacity, 0.5f + (i * 0.065f), Texture,
                    0, new Vector2((float)(Projectile.Center.X + Math.Sin((++Projectile.localAI[1] + i * 48f) * 0.015f) * 15f), Projectile.Center.Y - (i * 22.5f)), false, false, false);
            }
            return false;
        }
        public override void AI()
        {
            
            Player player = Main.player[Projectile.owner];
            float speed = 20;
            float speed2 = 1.05f;
            float speed3 = 0.125f;

            if (player.channel && player.ItemAnimationActive && player.HeldItem.type == ModContent.ItemType<WetTome>())
                Projectile.timeLeft = 5;

            if (Projectile.ai[2] != 0)
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (200 * Projectile.ai[2])) * speed2;

            Dust.NewDust(Projectile.Center - new Vector2(0, Main.rand.Next(0, 388)), 64, -400,
                ModContent.DustType<MuramasaDust>(), 0, 0, 0, default, 1f);

            Dust.NewDust(Projectile.Center - new Vector2(0, Main.rand.Next(0, 388)), 64, -400,
                ModContent.DustType<LandsiteDroplet>(), Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1.25f);

            rec = new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y - 350, 64, 350);

            if (++Timer2 > 30)
            {

                Vector2 targetVelocity = Projectile.DirectionTo(Main.MouseWorld + Vector2.One.RotatedByRandom(360) * Main.rand.Next(20, 180)) * speed;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, speed3);
            }

            if (Projectile.ai[0] == 0)
                Projectile.ai[0] = 1;

            /*Vector2 targetVelocity = Projectile.DirectionTo(npc.Center + Vector2.One.RotatedByRandom(360) * Main.rand.Next(20, 130)) * 11;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.15f);*/
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Intersects(rec))
                return true;
            else
                return false;
        }
    }
}