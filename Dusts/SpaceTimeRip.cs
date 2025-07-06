using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class SpaceTimeRip : ModDust
    {
        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/CosmicSphere";
        public int dustrot;
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 32, 32);
            dust.noGravity = true;
            dust.alpha = 0;
            dust.rotation = 0;
            dustrot = Main.rand.Next();
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SpaceStockImageByWikiImagesOnPixabay").Value;

            Vector2 pos = dust.position - Main.screenPosition;
            sb.Draw(sprite2, new Rectangle((int)pos.X - 43, (int)pos.Y, 86, 126), new Rectangle(745, 483, 152, 152), Color.White);

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Bloom").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            sb.Draw(sprite, dust.position - Main.screenPosition, null, dust.color * 0.5f,
                dustrot, origin, dust.scale / 3, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
