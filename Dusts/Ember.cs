using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Ember : ModDust
    {
        public int dustrot;
        public float size = 1;
        public static float velX;

        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/CosmicSphere";
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 16, 16);
            dust.noGravity = true;
            dustrot = Main.rand.Next();
            dust.fadeIn = dust.velocity.X;

            dust.noLight = true;
            dust.noLightEmittence = true;
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity.X = dust.fadeIn + (float)Math.Sin(Main.GlobalTimeWrappedHourly / 2f) * 2;
            Lighting.AddLight(dust.position, 0.25f, 0.175f, 0f);

            dust.scale *= 0.975f;
            if (dust.scale < 0.05f)
                dust.active = false;

            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            sb.Draw(sprite, dust.position - Main.screenPosition, null, Color.OrangeRed, dustrot, origin, dust.scale / 3f, SpriteEffects.None, 0);
            sb.Draw(sprite, dust.position - Main.screenPosition, null, Color.Yellow, dustrot, origin, dust.scale / 8f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
