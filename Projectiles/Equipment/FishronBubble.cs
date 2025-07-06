using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class FishronBubble : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG";
        public Player player;
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
        }
        public override void AI()
        {
            Projectile.Center = player.Center;
            if (player.wet && player.GetModPlayer<TranscendencePlayer>().FishronPerceptionAcc)
                Projectile.timeLeft = 45;

            if (Projectile.ai[2] < 1f)
                Projectile.ai[2] += 1f / 30f;

            if (Projectile.timeLeft < 30)
                Projectile.scale -= 1f / 30f;

            if (Projectile.timeLeft > 30 && Projectile.scale < 1f)
                Projectile.scale += 1f / 10f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Space Texture
            string string1 = "TranscendenceMod/Miscannellous/Assets/WaterShader";
            if (Projectile.ai[1] == 1)
                string1 = "TranscendenceMod/Miscannellous/Assets/MasterModeShader";
            if (Projectile.ai[1] == 2)
                string1 = "TranscendenceMod/Miscannellous/Assets/OiledShader";
            if (Projectile.ai[1] == 3)
                string1 = "TranscendenceMod/Miscannellous/Assets/SeraphFontShader";

            Texture2D shaderImage = ModContent.Request<Texture2D>(string1).Value;
            Texture2D shaderImage2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack").Value;

            eff.Parameters["uImageSize0"].SetValue(sprite.Size());
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 0.5f);
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uTime"].SetValue(0.5f);
            eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly / 2f);

            eff.Parameters["useAlpha"].SetValue(true);
            eff.Parameters["alpha"].SetValue(0.75f * Projectile.ai[2]);
            eff.Parameters["useExtraCol"].SetValue(false);

            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, eff, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, 2f * Projectile.ai[2] * Projectile.scale, Texture, 0, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, eff, Main.GameViewMatrix.TransformationMatrix);

            Main.instance.GraphicsDevice.Textures[1] = shaderImage2;
            eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly);
            eff.Parameters["useExtraCol"].SetValue(true);
            Vector3 vec3 = new Vector3(0f, 0.2f, 1f);

            if (Projectile.ai[1] == 1)
                vec3 = new Vector3(1f, 0.2f, 0f);

            if (Projectile.ai[1] == 2)
                vec3 = new Vector3(1f, 0.6f, 0f);

            if (Projectile.ai[1] == 3)
                vec3 = new Vector3(Main.DiscoR / 255f, Main.DiscoG / 255f, Main.DiscoB / 255f);

            eff.Parameters["extraCol"].SetValue(vec3);

            for (int i = 0; i < 3; i++)
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 2f * Projectile.ai[2] * Projectile.scale, Texture, 0, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, default, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            player = Main.player[Projectile.owner];

            if (player.lavaWet)
                Projectile.ai[1] = 1;
            if (player.honeyWet)
                Projectile.ai[1] = 2;
            if (player.shimmerWet)
                Projectile.ai[1] = 3;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }
}