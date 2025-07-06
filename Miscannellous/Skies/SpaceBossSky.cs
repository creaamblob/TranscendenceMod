using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Threading;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using TranscendenceMod.NPCs.Boss.Seraph;
using static tModPorter.ProgressUpdate;

namespace TranscendenceMod.Miscannellous.Skies
{
    //Making custom skies be like: *Insert turkish ice cream man GIF here*
    public class SpaceBossSky : CustomSky
    {
        private bool active = false;
        private float fadeIn = 0;
        public Vector2[] starsPos = new Vector2[400];
        public Vector2[] starsVel = new Vector2[400];
        public float[] starsSize = new float[400];
        public float[] starsRot = new float[400];
        public Color[] starsColour = new Color[400];

        public override void Activate(Vector2 position, params object[] args)
        {
            if (!active)
            {
                for (int i = 0; i < 400; i++)
                {
                    starsPos[i] = new Vector2(Main.rand.Next(0, 1920), Main.rand.Next(0, 1080));
                    starsVel[i] = new Vector2(Main.rand.NextFloat(2f, 4f), Main.rand.NextFloat(4f, 6f));

                    starsSize[i] = Main.rand.NextBool(8) ? Main.rand.NextFloat(2f, 2.275f) : 
                        Main.rand.NextBool(10) ? Main.rand.NextFloat(1f, 1.375f) :
                        Main.rand.NextFloat(0.325f, 0.66f);

                    starsVel[i] *= MathHelper.Lerp(1f, 2.5f, starsSize[i] / 2f);

                    if (Main.rand.NextBool(96))
                    {
                        starsSize[i] = 10f;
                        starsVel[i].X = Main.rand.NextFloat(-4f, 4f);
                        starsVel[i].Y = 15f;
                    }

                    bool PrideMonth = DateTime.Now.Month == 6;
                    if (PrideMonth)
                    {
                        switch (Main.rand.Next(0, 3))
                        {
                            case 0: starsColour[i] = Color.Aqua; break;
                            case 1: starsColour[i] = Color.Pink; break;
                            case 2: starsColour[i] = Color.White; break;
                        }
                    }
                    else
                    {
                        starsColour[i] = Color.DeepSkyBlue;
                        if (Main.rand.NextBool(3))
                            starsColour[i] = Color.Aqua;
                        if (Main.rand.NextBool(4))
                            starsColour[i] = Color.HotPink;
                        if (Main.rand.NextBool(4))
                            starsColour[i] = Color.Lime;
                        if (Main.rand.NextBool(6))
                            starsColour[i] = Color.Yellow;
                    }


                    starsRot[i] = Main.rand.NextFloat(-0.2f, 0.2f);
                }
                active = true;
            }
        }
        public override void Deactivate(params object[] args)
        {
            active = false;
        }
        public override bool IsActive() => fadeIn > 0f;
        public override void Update(GameTime gameTime)
        {
            if (fadeIn < 1 && active)
                fadeIn += 0.025f;
            if (!active && fadeIn > 0)
                fadeIn -= 0.025f;
        }
        public override Color OnTileColor(Color inColor)
        {
            int npC = NPC.FindFirstNPC(ModContent.NPCType<CelestialSeraph>());
            if (npC != -1)
            {
                NPC npc = Main.npc[npC];
                if (npc != null && npc.type == ModContent.NPCType<CelestialSeraph>() && npc.ModNPC is CelestialSeraph boss && boss != null)
                    return Color.Lerp(TranscendenceWorld.CosmicPurple * 0.4f, inColor, 1 - fadeIn);
            }
            return base.OnTileColor(inColor);
        }
        public override void Reset() => active = false;
        public override float GetCloudAlpha()
        {
            return 1 - fadeIn;
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            int npC = NPC.FindFirstNPC(ModContent.NPCType<CelestialSeraph>());
            if (npC != -1)
            {
                NPC npc = Main.npc[npC];
                if (npc != null && npc.type == ModContent.NPCType<CelestialSeraph>() && npc.ModNPC is CelestialSeraph boss && boss != null)
                {
                    Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/OrionNebula").Value;


                    Texture2D star = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
                    Texture2D star2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/StarEffect").Value;



                    if (maxDepth >= 0 && minDepth < 0)
                    {
                        if (boss.DeathAnim)
                        {
                            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * fadeIn);
                            return;
                        }

                        Vector3 col = new Vector3(0.75f, 0, 1f);

                        spriteBatch.Draw(sprite, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(col.X, col.Y, col.Z) * fadeIn);

                        //Draw the vignette
                        var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphSkyShader", AssetRequestMode.ImmediateLoad).Value;
                        eff.Parameters["uColor"].SetValue(col * fadeIn);

                        eff.Parameters["uOpacity"].SetValue(8f * fadeIn);
                        eff.Parameters["uImageSize0"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f);
                        eff.Parameters["uTime"].SetValue(-TranscendenceWorld.UniversalRotation * 0.15f);
                        //Apply Space Textures
                        Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack").Value;
                        eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 2f);
                        Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                        spriteBatch.End();
                        spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff);

                        spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

                        spriteBatch.End();
                        spriteBatch.Begin(default, BlendState.Additive, default, default, default, null);

                        for (int i = 0; i < 400; i++)
                        {
                            float twinkle = 0f;

                            if (i % 8 == 0)
                            {
                                twinkle = 1f + (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 12f) * 0.25f);
                                if (twinkle < 0.8f)
                                    twinkle = 0f;
                            }

                            if (starsPos[i].X > 1930)
                                starsPos[i].X = -10;

                            if (starsPos[i].X < -20)
                                starsPos[i].X = 1930;

                            if (starsPos[i].Y > 1090)
                                starsPos[i].Y = -10;

                            if (starsSize[i] < 10f)
                            {
                                starsPos[i] += starsVel[i] * 0.2f;

                                spriteBatch.Draw(star, starsPos[i], null, starsColour[i] * 0.5f, starsRot[i], star.Size() * 0.5f, (starsSize[i] / 6f), SpriteEffects.None, 0);

                                spriteBatch.Draw(star, starsPos[i], null, Color.White, starsRot[i], star.Size() * 0.5f, starsSize[i] / 20f, SpriteEffects.None, 0);
                                spriteBatch.Draw(star, starsPos[i], null, Color.White, starsRot[i], star.Size() * 0.5f, starsSize[i] / 20f, SpriteEffects.None, 0);

                                spriteBatch.Draw(star2, starsPos[i], null, Color.White * 0.66f, starsRot[i] + Main.GlobalTimeWrappedHourly * 2, star2.Size() * 0.5f, starsSize[i] * 0.75f * twinkle, SpriteEffects.None, 0);
                            }
                            else
                            {
                                starsPos[i] += starsVel[i] * 0.5f;

                                for (int j = 0; j < 32; j++)
                                {
                                    float fade = MathHelper.Lerp(1f, 0.5f, j / 32f);

                                    spriteBatch.Draw(star, starsPos[i] - (starsVel[i] * (j * 0.2f)), null, Color.Lerp(Color.DeepSkyBlue * 0.5f, Color.Transparent, j / 38f), starsRot[i], star.Size() * 0.5f, starsSize[i] / 70f * fade, SpriteEffects.None, 0);
                                    spriteBatch.Draw(star, starsPos[i] - (starsVel[i] * (j * 0.2f)), null, Color.Lerp(Color.White, Color.Transparent, j / 38f), starsRot[i], star.Size() * 0.5f, starsSize[i] / 120f * fade, SpriteEffects.None, 0);
                                }
                            }

                        }

                        spriteBatch.End();
                        spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null);

                    }


                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, null);
                    
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                            null, Color.Black * 0.75f * boss.skyFade);


                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null);

                }
            }
        }
    }
}


