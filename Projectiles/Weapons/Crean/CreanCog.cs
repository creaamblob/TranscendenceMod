using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Crean
{
    public class CreanCog : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Projectile.rotation += 0.175f * Projectile.scale;
            Vector2 vel = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel;

            Player player = Main.player[Projectile.owner];
            Projectile.tileCollide = Projectile.ai[2] == 1 && Projectile.ai[1] != 1;

            if (!(player != null && player.active && !player.dead && player.HeldItem.type == ModContent.ItemType<CreanStaff>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems))
                Projectile.Kill();

            if (player.controlUseItem)
                Projectile.ai[2] = 1;

            if (++Projectile.ai[0] < 30 || Projectile.ai[2] == 0)
            {
                Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel = player.DirectionTo(Main.MouseWorld) * 24f;
                Projectile.timeLeft = 300;

                float dist = Projectile.ai[0] > 15 ? 150f : Projectile.ai[0] * 10f;
                Projectile.Center = player.Center + Vector2.One.RotatedBy(vel.ToRotation() - MathHelper.PiOver4) * dist;

                return;
            }

            if (Projectile.Distance(player.Center) > 500)
                Projectile.ai[1] = 1;

            if (Projectile.ai[1] == 1)
            {
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 1f / 20f);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center) * 20f, 1f / 10f);

                if (Projectile.Distance(player.Center) < 50)
                    Projectile.Kill();
            }
            else
            {
                Projectile.scale += 1f / 5f;
                Projectile.velocity = vel;
            }

        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (30 * Projectile.scale))
                return true;
            else return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[1] = 1;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (Projectile.ai[2] == 1)
                Projectile.ai[1] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {

            if (Projectile.ai[0] < 30 || Projectile.ai[2] == 0)
            {
                Player player = Main.player[Projectile.owner];
                SpriteBatch spriteBatch = Main.spriteBatch;

                if (player != null && player.active)
                {
                    Texture2D sprite = ModContent.Request<Texture2D>(Texture + "_Chain").Value;


                    spriteBatch.Draw(sprite, new Rectangle(
                        (int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), 12,
                        (int)(Projectile.Distance(player.Center) * 2)), null,
                        Color.White * 0.33f, Projectile.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
                }
            }

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 0f, 0.5f, 0.75f, 0.5f, false);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}