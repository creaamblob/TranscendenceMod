using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class OvergrownChain : ModProjectile
    {
        public Player player;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 180;

            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }
        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);  
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = TextureAssets.Chain5.Value;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];

                if (projectile != null && projectile.active && projectile.Center.Distance(Projectile.Center) < 350 && projectile.type == Type)
                {
                    sb.End();
                    sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    int dist = (int)Projectile.Distance(projectile.Center);
                    float rot = Projectile.DirectionTo(projectile.Center).ToRotation() + MathHelper.PiOver2;
                    Vector2 pos = Vector2.Lerp(Projectile.Center, projectile.Center, 0.5f);

                    float alpha = projectile.scale > 0.9f ? Projectile.scale : projectile.scale;

                    sb.Draw(sprite, new Rectangle(
                        (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(16 * Projectile.scale), dist), null,
                        Color.Lime * 0.5f * alpha, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                    sb.End();
                    sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }

            return base.PreDraw(ref lightColor);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];

                if (projectile != null && projectile.active && projectile.Center.Distance(Projectile.Center) < 350 && projectile.type == Type)
                {
                    if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, projectile.Center, 5, ref reference))
                        return true;
                    else return base.Colliding(projHitbox, targetHitbox);
                }
            }
            
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (player == null || !player.active)
                return;

            if (Projectile.timeLeft < 60 && Projectile.scale > 0)
                Projectile.scale -= 1f / 60f;
        }
    }
}