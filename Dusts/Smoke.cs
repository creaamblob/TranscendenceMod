using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Smoke : ModDust
    {
        public int dustrot;
        public int frameY = 0;
        public static int Timer;
        public float alpha;

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            frameY = Main.rand.NextFromList<int>(0, 200, 400);
        }
        public override bool Update(Dust dust)
        {
            dust.rotation += 0.01f;
            dust.scale -= 0.0025f;
            if (++Timer > 30)
            {
                if (++dust.alpha > 255)
                    dust.active = false;
            }
            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            sb.Draw(sprite, dust.position - new Vector2(200, -200) - Main.screenPosition, new Rectangle(0, frameY, 250, 200), new Color(dust.color.R, dust.color.G, dust.color.B, 0f) * (1 - (dust.alpha / 255f)), dust.rotation, origin, dust.scale * 0.5f, SpriteEffects.None, 0);
            return false;
        }
    }
}
