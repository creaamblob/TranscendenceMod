using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Smoke3 : ModDust
    {
        public int dustrot;
        public int frameY = 0;
        public int Timer;
        public float alpha;

        public override string Texture => "TranscendenceMod/Dusts/Smoke";

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.fadeIn = Main.rand.NextBool(2) ? 200 : 0;
        }
        public override bool Update(Dust dust)
        {
            dust.position.X += dust.velocity.X;
            dust.position.Y += dust.velocity.Y;
            dust.scale -= 0.0075f;

            if (++Timer > 10)
            {
                dust.alpha += 25;
                if (dust.alpha > 255)
                    dust.active = false;
            }
            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            sb.Draw(sprite, dust.position + (new Vector2(0, 100) * dust.scale) - Main.screenPosition, new Rectangle(0, (int)dust.fadeIn, 250, 200), new Color(dust.color.R, dust.color.G, dust.color.B, 0) * (1 - (dust.alpha / 255f)), dust.rotation, origin, dust.scale * 0.5f, SpriteEffects.None, 0);
            return false;
        }
    }
}
