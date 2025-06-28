using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class SnowflakeDust : ModDust
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
            dust.rotation += MathHelper.ToRadians(5);
            dust.velocity.X *= 1.05f;

            if (++dust.fadeIn > 30)
            {
                dust.velocity.Y += Main.rand.NextFloat(0.25f, 1.75f);

                dust.scale *= 0.925f;
                if (dust.scale < 0.1f)
                    dust.active = false;
            }
            return base.Update(dust);
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Projectiles/NPCs/Bosses/Dragon/Snow").Value;

            sb.Draw(sprite, dust.position - Main.screenPosition, null, Color.DeepSkyBlue * 0.75f, dustrot, sprite.Size() * 0.5f, dust.scale / 5f, SpriteEffects.None, 0);
            sb.Draw(sprite2, dust.position - Main.screenPosition, null, Color.White, dustrot, sprite2.Size() * 0.5f, dust.scale / 3f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
