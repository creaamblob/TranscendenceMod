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

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(ent, Color.Orange * 0.75f, scale * 1.75f, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleAura",
                MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * -160), ent.Center, null);

            TranscendenceUtils.DrawEntity(ent, Color.OrangeRed, scale, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleAura",
                MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 120), ent.Center, null);

            TranscendenceUtils.DrawEntity(ent, Color.White, scale, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleAura",
                MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 260), ent.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(ent, Color.Black, scale * 0.25f, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleCenter",
                MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * -120), ent.Center, null);
        }
    }
}

