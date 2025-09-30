using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class DashLaser : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public Vector2 EndPos;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/Trail2";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.scale = 0f;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.alpha = 255;
            if (Projectile.timeLeft < 48) Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 100;

            if (Main.npc[(int)Projectile.ai[1]] != null && Projectile.timeLeft > 48)
                EndPos = Main.npc[(int)Projectile.ai[1]].Center;

            if (Projectile.timeLeft > 40)
                Projectile.scale += 1f / 20f;

            if (Projectile.timeLeft < 30 && Projectile.scale > 0)
                Projectile.scale -= 1f / 30f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;

            SoundEngine.PlaySound(SoundID.Item71 with { MaxInstances = 0 });

            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, EndPos, 5, ref reference))
                return true;
            else return false;
        }
        public override bool? CanDamage() => Projectile.timeLeft > 25;
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 pos = Vector2.Lerp(Center, EndPos, 0.5f);

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(140 * Projectile.scale), (int)(Center.Distance(EndPos) * 2f)), null,
                Color.Red * Projectile.scale, Center.DirectionTo(EndPos).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(80 * Projectile.scale), (int)(Center.Distance(EndPos) * 2f)), null,
                Color.White * Projectile.scale, Center.DirectionTo(EndPos).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}