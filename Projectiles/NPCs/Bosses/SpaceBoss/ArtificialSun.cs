using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class ArtificialSun : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 256;
            Projectile.height = 256;
            Projectile.timeLeft = 3600;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            if (Main.npc[(int)Projectile.ai[1]].ModNPC is CelestialSeraph boss)
                DrawBreathingStar(Projectile, boss.Timer_AI > (boss.AttackDuration - 120) ? 1f : boss.skyFade, spriteBatch, true);
            return false;
        }
        public static void DrawBreathingStar(Projectile Projectile, float scale, SpriteBatch spriteBatch, bool Bloom)
        {
            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphSunShader", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Perlin2").Value;
            Texture2D shaderImage2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;


            Main.instance.GraphicsDevice.Textures[1] = shaderImage;
            Main.instance.GraphicsDevice.Textures[2] = shaderImage2;



            eff.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 0.4f);
            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size() * 0.125f);
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 0.25f);
            eff.Parameters["uImageSize2"].SetValue(shaderImage2.Size() * 0.5f);


            if (Bloom)
            {
                TranscendenceUtils.DrawEntity(Projectile, Color.Yellow * 0.75f, 40f * scale, "bloom", 0, Projectile.Center, null);
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 20f * scale, "bloom", 0, Projectile.Center, null);
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);


            TranscendenceUtils.DrawEntity(Projectile, Color.White, scale * 1175f, "TranscendenceMod/Miscannellous/Assets/Pixel", 0f, Projectile.Center, null);


            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            if (Main.npc[(int)Projectile.ai[1]].ModNPC is CelestialSeraph boss)
                Projectile.timeLeft = boss.AttackDuration;
        }

        public override void AI()
        {
            if (Main.npc[(int)Projectile.ai[1]].ModNPC is CelestialSeraph boss)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player p = Main.player[i];
                    if (p != null && p.active && p.Distance(Projectile.Center) < (boss.skyFade * 375f))
                        p.AddBuff(ModContent.BuffType<SunMelt>(), 2);
                }
            }
        }
    }
}