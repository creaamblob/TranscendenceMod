using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class TrailDustHQ : ModDust
    {
        public int dustrot;
        public float size;
        public Vector2 vel;

        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/CosmicSphere";

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 16, 16);
            dust.noGravity = true;
            size = dust.scale;
            dust.scale = 0f;
            dust.velocity *= 0.01f;
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.alpha = 0;

            if (dust.frame.Y < 8)
            {
                dust.frame.Y++;
                return false;
            }

            if (dust.scale > size)
            {
                if (++dust.fadeIn > 5)
                    dust.active = false;
            }
            else dust.scale += 0.075f;
            return false;
        }
        public override bool PreDraw(Dust dust)
        {

            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            for (int i = 0; i < 10; i++)
            {
                float mult = MathHelper.Lerp(0f, MathHelper.Lerp(2f, 0.1f, dust.scale / size), dust.frame.Y / 8f);
                sb.Draw(sprite, dust.position - dust.velocity * (i * mult) - Main.screenPosition, null, Color.Lerp(dust.color * 0.75f, Color.Transparent, dust.fadeIn / 5f), dustrot, origin, dust.scale / 4 * (1 - i / 15f), SpriteEffects.None, 0);
                sb.Draw(sprite, dust.position - dust.velocity * (i * mult) - Main.screenPosition, null, Color.Lerp(Color.White, Color.Transparent, dust.fadeIn / 5f), dustrot, origin, dust.scale * 0.1f * (1 - i / 15f), SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
