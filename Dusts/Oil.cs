using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Oil : GravityCollisionDust
    {

        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/CosmicSphere";

        public override int TimeUntilFade => 180;

        public override int FadeSpeed => 6;

        public override float MaxGrow => 1.25f;

        public override int HitboxSize => 4;

        public override float FallSpeed => 0.175f;

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 16, 16);
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Circle").Value;
            Vector2 origin = new Vector2(sprite2.Width * 0.5f, sprite2.Height * 0.5f);

            spriteBatch.Draw(sprite2, dust.position - Main.screenPosition, null, Color.Black, dust.rotation, origin, dust.scale, SpriteEffects.None, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
