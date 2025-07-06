using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class BetterBlood : ModDust
    {

        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/CosmicSphere";


        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;

            dust.velocity.Y += 0.1750f;
            dust.scale *= 0.9925f;

            if (dust.scale < 0.05f)
                dust.active = false;
            return false;
        }
        

        public override bool PreDraw(Dust dust)
        {

            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.NonPremultiplied, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            for (int i = 0; i < 8; i++)
            {
                sb.Draw(sprite, dust.position - dust.velocity * (i / 4f) - Main.screenPosition, null, new Color(1f, 0f, 0f, 1 - (dust.alpha / 255f)) * Lighting.Brightness((int)(dust.position.X / 16), (int)dust.position.Y / 16) * 1.5f, dust.rotation, origin, dust.scale * 0.1f * (1 - i / 10f), SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
