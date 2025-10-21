using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Horsespark : GravityCollisionDust
    {

        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/CosmicSphere";

        public override int TimeUntilFade => 15;

        public override int FadeSpeed => 15;

        public override float MaxGrow => 1f;

        public override int HitboxSize => 4;

        public override float FallSpeed => 0.2f;

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 16, 16);
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/StarBloom").Value;
            Vector2 origin = new Vector2(sprite2.Width * 0.5f, sprite2.Height * 0.5f);

            spriteBatch.Draw(sprite2, dust.position - Main.screenPosition, null, dust.color * 0.5f * (1f - (dust.alpha / 255f)), dust.rotation, origin, dust.scale * 0.33f, SpriteEffects.None, 0);
            spriteBatch.Draw(sprite2, dust.position - Main.screenPosition, null, Color.White * (1f - (dust.alpha / 255f)), dust.rotation * 2f, origin, dust.scale * 0.175f, SpriteEffects.None, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
