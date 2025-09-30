using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Smoke : ModDust
    {
        public override string Texture => TranscendenceMod.ASSET_PATH + "/Smoke";

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame.Y = Main.rand.NextFromList<int>(0, 200, 400);
        }
        public override bool Update(Dust dust)
        {
            dust.rotation += 0.15f;
            dust.position.X += dust.velocity.X;
            dust.position.Y += dust.velocity.Y;
            dust.scale -= 0.005f;

            if (++dust.fadeIn >= 15)
            {
                dust.alpha += 10;
                if (dust.alpha > 255)
                    dust.active = false;
            }
            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Rectangle rec = new Rectangle(0, dust.frame.Y, 250, 200);
            Vector2 origin = rec.Size() * 0.5f;
            sb.Draw(sprite, dust.position - Main.screenPosition, rec, dust.color * (1 - (dust.alpha / 255f)), dust.rotation, origin, dust.scale * 0.5f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
