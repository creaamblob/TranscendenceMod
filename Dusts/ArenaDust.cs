using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class ArenaDust : ModDust
    {
        public int dustrot;
        public float size = 1;

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 16, 16);
            dust.noGravity = true;
            dust.noLight = true;
            dustrot = Main.rand.Next();
        }
        public override bool Update(Dust dust)
        {
            return base.Update(dust);
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            Lighting.AddLight(dust.position, dust.color.R / 255f, dust.color.G / 255f, dust.color.B / 255f);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;



            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uImageSize1"].SetValue(new Vector2(sprite.Width * (dust.scale / 3f), sprite.Height * (dust.scale / 3f)));
            eff.Parameters["maxColors"].SetValue(12);


            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            sb.Draw(sprite, dust.position - Main.screenPosition, null, dust.color * 0.75f, dustrot, origin, dust.scale / 3f, SpriteEffects.None, 0);
            sb.Draw(sprite, dust.position - Main.screenPosition, null, Color.White, dustrot, origin, dust.scale * 0.166666666667f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
