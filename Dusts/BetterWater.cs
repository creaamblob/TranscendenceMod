using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class BetterWater : ModDust
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/RainDrop").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            dust.rotation = dust.position.AngleTo(dust.velocity);

            Main.EntitySpriteDraw(sprite, dust.position - Main.screenPosition, null, Color.White, dust.rotation, origin, dust.scale * 0.3f, SpriteEffects.None);

            return base.PreDraw(dust);
        }
    }
}
