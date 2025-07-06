using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class SpaceBossBlood : ModDust
    {
        public int dustrot;
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 10, 10);
            dustrot = Main.rand.Next();
        }
        public override Color? GetAlpha(Dust dust, Color lightColor) => Color.HotPink;
        /*public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, blendState: BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Circle",
                AssetRequestMode.ImmediateLoad).Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            Main.EntitySpriteDraw(sprite, dust.position + new Vector2(Main.rand.Next(-3, 3)) - Main.screenPosition, null, Color.MediumVioletRed, dustrot,
                origin, dust.scale * 1.1f, SpriteEffects.None);
            return base.PreDraw(dust);
        }*/
    }
}
