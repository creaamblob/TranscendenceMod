using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class PlayerCosmicBlood : ModDust
    {
        public int dustrot;
        public float size = 1;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloom";
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 16, 16);
            dust.noGravity = true;
            dustrot = Main.rand.Next();
        }
        public override bool Update(Dust dust)
        {
            dust.velocity *= 0.9f;
            dust.position += dust.velocity;

            if (++dust.alpha > 60)
            {
                dust.scale *= 0.9f;
                if (dust.scale < 0.1f)
                    dust.active = false;
            }

            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Pixel").Value;

            sb.Draw(sprite, dust.position - Main.screenPosition, null, dust.color * 0.5f, 0, sprite.Size() * 0.5f, dust.scale / 4f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(sprite2, dust.position + Main.rand.NextVector2Square(-2, 2) - Main.screenPosition, null, Color.White, 0, sprite2.Size() * 0.5f, dust.scale * 1.5f, SpriteEffects.None, 0);
            return false;
        }
    }
}
