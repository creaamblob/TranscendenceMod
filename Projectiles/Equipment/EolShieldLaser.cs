using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class EolShieldLaser : ModProjectile
    {
        public int Timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 120;

            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            AIType = ProjectileID.Bullet;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite4 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Trail2").Value;
            Vector2 origin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

            SpriteBatch sb = Main.spriteBatch;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            string string1 = "TranscendenceMod/Miscannellous/Assets/RainbowShader";
            Texture2D shaderImage = ModContent.Request<Texture2D>(string1).Value;

            Projectile.localAI[2] += 0.0125f;

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 0.25f);
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uTime"].SetValue(Projectile.localAI[2] * 5f);
            eff.Parameters["yChange"].SetValue(Projectile.localAI[2] * 5f);

            eff.Parameters["useExtraCol"].SetValue(true);
            eff.Parameters["extraCol"].SetValue(new Vector3(Main.DiscoR / 255f, Main.DiscoG / 255f, Main.DiscoB / 255f));

            float a = Projectile.ai[1];
            eff.Parameters["useAlpha"].SetValue(true);
            eff.Parameters["alpha"].SetValue(a);

            for (int i = 2; i < ((Projectile.oldPos.Length / 2)); i++)
            {
                Vector2 pos = Projectile.oldPos[i * 2] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);

                Main.EntitySpriteDraw(sprite4, pos, null, Color.White * a, Projectile.oldRot[i * 2] + MathHelper.PiOver2, sprite4.Size() * 0.5f, Projectile.scale * 3f, SpriteEffects.None);
            }

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 4; i < ((Projectile.oldPos.Length - 2)); i++)
            {
                Vector2 pos = Projectile.oldPos[i] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);

                Main.EntitySpriteDraw(sprite4, pos, null, Color.White * a, Projectile.oldRot[i] + MathHelper.PiOver2, sprite4.Size() * 0.5f, Projectile.scale * 1f, SpriteEffects.None);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (targetHitbox.Distance(Projectile.oldPos[i]) < 5 && Projectile.active)
                    return true;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.spriteDirection = (int)Projectile.velocity.ToRotation();

            if (Projectile.timeLeft > 90 && Projectile.ai[1] < 1f)
                Projectile.ai[1] += 0.0125f;
            else if (Projectile.timeLeft < 60 && Projectile.ai[1] > 0f)
                Projectile.ai[1] -= 1f / 60f;

            if (++Timer > 3)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (20 * Projectile.ai[2]));
                Timer = 0;
            }
        }
    }
}