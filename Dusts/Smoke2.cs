using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Smoke2 : ModDust
    {
        public int dustrot;
        public int frameY = 0;
        public int Timer;
        public float alpha;

        public override string Texture => "TranscendenceMod/Dusts/Smoke";

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            frameY = Main.rand.NextFromList<int>(0, 200, 400);
        }
        public override bool Update(Dust dust)
        {
            dust.rotation += 0.15f;
            dust.position.X += dust.velocity.X;
            dust.position.Y += dust.velocity.Y;
            dust.scale -= 0.005f;

            if (++Timer > 15)
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
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            sb.Draw(sprite, dust.position - new Vector2(100, 100) - Main.screenPosition, new Rectangle(0, frameY, 250, 200), dust.color * (1 - (dust.alpha / 255f)), dust.rotation, origin, dust.scale * 0.5f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
