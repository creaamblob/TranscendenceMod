using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Palladium2 : ModDust
    {
        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/CosmicSphere";
        public int dustrot;
        public float size = 1;

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 16, 16);
            dust.noGravity = true;
            dustrot = Main.rand.Next();
        }
        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position + new Vector2(8 * dust.scale), 0.5f, 0.33f, 0f);
            return base.Update(dust);
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.NonPremultiplied, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Bloom").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            sb.Draw(sprite, dust.position - Main.screenPosition, null, Color.OrangeRed, dustrot, origin, dust.scale * 0.25f, SpriteEffects.None, 0);
            sb.Draw(sprite, dust.position - Main.screenPosition, null, Color.Yellow, dustrot, origin, dust.scale * 0.166666666667f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
