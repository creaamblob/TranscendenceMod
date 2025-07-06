using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class MoonFrag : ModProjectile
    {
        public int Timer;
        public float ChaseSpeed = 12.5f;
        public Vector2 pos;
        private Texture2D sprite2;
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 360;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            bool OnScreen = Projectile.Center.Between(Main.screenPosition - new Vector2(Main.screenWidth * 1.2f, Main.screenHeight * 1.2f), Main.screenPosition + new Vector2(Main.screenWidth * 1.2f, Main.screenHeight * 1.2f));

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile != null && projectile.active && projectile.Center.Distance(Projectile.Center) < 1000 && OnScreen 
                    && projectile.type == Type && Projectile.GetGlobalProjectile<TranscendenceProjectiles>().SpaceBossPortalProjectile > 1 &&
                    Projectile.GetGlobalProjectile<TranscendenceProjectiles>().SpaceBossPortalProjectile == projectile.GetGlobalProjectile<TranscendenceProjectiles>().SpaceBossPortalProjectile &&
                    Projectile.GetGlobalProjectile<TranscendenceProjectiles>().StellarDirection == projectile.GetGlobalProjectile<TranscendenceProjectiles>().StellarDirection + 1 &&
                    Projectile.GetGlobalProjectile<TranscendenceProjectiles>().StupidInt == projectile.GetGlobalProjectile<TranscendenceProjectiles>().StupidInt)
                {
                    Vector2 pos2 = Projectile.Center - Main.screenPosition;
                    Rectangle rec = new Rectangle((int)pos2.X, (int)pos2.Y, 20, (int)Projectile.Distance(projectile.Center) * 2);

                    if (sprite2 == null)
                        sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine2").Value;

                    sb.End();
                    sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    Main.spriteBatch.Draw(sprite2, rec, null, Color.White * 0.5f, Projectile.DirectionTo(projectile.Center).ToRotation() - MathHelper.PiOver2, default, default, 0);

                    sb.End();
                    sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }

            if (Timer < (15 + Projectile.ai[2]) && Projectile.ai[0] == 150 && pos != Vector2.Zero)
            {
                if (Projectile.Distance(pos) < 5000)
                {
                    Vector2 pos2 = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().startPos;
                    float Lenght = 2;

                    Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

                    sb.End();
                    sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    sb.Draw(sprite, new Rectangle(
                        (int)(pos2.X - Main.screenPosition.X),
                        (int)(pos2.Y - Main.screenPosition.Y),
                        25,
                        (int)(pos2.Distance(pos) * Lenght)), null,
                        Color.SlateBlue * 0.75f, pos2.DirectionTo(pos).ToRotation() + MathHelper.PiOver2,
                        sprite.Size() * 0.5f,
                        SpriteEffects.None,
                        0);

                    sb.End();
                    sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 1f, 1f, 1f, false);

            return base.PreDraw(ref lightColor);
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
    }
}