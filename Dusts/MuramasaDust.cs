using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class MuramasaDust : ModDust
    {
        public int dustrot;
        public float size = 1;

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 18, 18);
            dust.noGravity = true;
            dustrot = Main.rand.Next();
        }
        public override bool Update(Dust dust)
        {
            return base.Update(dust);
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>(Texture).Value;

            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uImageSize1"].SetValue(new Vector2(sprite.Width * (dust.scale * 0.75f), sprite.Height * (dust.scale * 0.75f)));
            eff.Parameters["maxColors"].SetValue(8);

            sb.End();
            sb.Begin(default, default, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);


            sb.Draw(sprite, dust.position - Main.screenPosition, null, new Color(0f, 0f, 1f, 0f) * 0.25f, dustrot, sprite.Size() * 0.5f, dust.scale * 0.75f, SpriteEffects.None, 0);
            sb.Draw(sprite, dust.position - Main.screenPosition, null, new Color(0f, 0.75f, 1f, 0f) * 0.5f, dustrot, sprite.Size() * 0.5f, dust.scale * 0.25f, SpriteEffects.None, 0);


            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);



            sb.Draw(sprite2, dust.position - Main.screenPosition, null, Color.White, dustrot, sprite2.Size() * 0.5f, dust.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
