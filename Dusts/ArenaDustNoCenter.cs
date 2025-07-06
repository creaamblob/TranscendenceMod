using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class ArenaDustNoCenter : ModDust
    {
        public int dustrot;
        public float size = 1;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 16, 16);
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
            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            sb.Draw(sprite, dust.position - Main.screenPosition, null, dust.color, dustrot, origin, dust.scale / 3, SpriteEffects.None, 0);
            sb.Draw(sprite, dust.position - Main.screenPosition, null, dust.color, dustrot, origin, dust.scale / 6, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
