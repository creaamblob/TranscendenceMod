using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class BoAExplosion : ModProjectile
    {
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 8;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 18;

            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.friendly = true;
            Projectile.light = 1.75f;
            Projectile.scale = 4;
        }
        //new Color(Main.rand.Next(122, 255), Main.rand.Next(122, 255), Main.rand.Next(122, 255));
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 95 * Projectile.scale)
                return true;
            else return base.Colliding(projHitbox, targetHitbox);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
            target.AddBuff(BuffID.Ichor, 300);
        }
        public override void AI()
        {
            if (++Projectile.ai[2] > 1)
            {
                Projectile.frame++;
                Projectile.ai[2] = 0;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Vector2 pos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
            int frameStuff = sprite.Height / Main.projFrames[Type];
            int y = frameStuff * Projectile.frame;
            Rectangle rec = new Rectangle(0, y, sprite.Width, frameStuff);
            Main.EntitySpriteDraw(sprite, pos, rec, Color.White, 0, rec.Size() * 0.5f, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}