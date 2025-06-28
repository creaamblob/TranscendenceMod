using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using ReLogic.Content;

namespace TranscendenceMod
{
    public class BlackholeDrawer : ModSystem
    {
        public static void DrawBlackhole(Entity ent, float scale, SpriteBatch spriteBatch)
        {

            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uImageSize1"].SetValue(new Vector2(2000, 2000));
            eff.Parameters["maxColors"].SetValue(8);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(ent, Color.OrangeRed * 0.66f, scale * 1.5f, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleAura",
                MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * -160), ent.Center, null);

            TranscendenceUtils.DrawEntity(ent, Color.Orange, scale, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleAura",
                MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 160), ent.Center, null);

            TranscendenceUtils.DrawEntity(ent, Color.White, scale * 0.85f, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleAura",
                MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 260), ent.Center, null);

            TranscendenceUtils.DrawEntity(ent, Color.White, scale * 0.4f, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHole2",
                MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 120), ent.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(ent, Color.Black, scale * 0.3f, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleCenter",
                MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * -120), ent.Center, null);
        }
    }
}

