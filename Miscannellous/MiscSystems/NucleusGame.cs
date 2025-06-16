using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Miscanellous.MiscSystems
{
    public class NucleusGame : ModPlayer
    {
        // 4:3 Aspect Ratio, 16 x 12
        /// <summary>
        /// 1 = Player<br></br>
        /// 2 = Tile<br></br>
        /// 3 = Spike
        /// 4 = Door
        /// </summary>
        public int[] pixel = new int[192];
        public int room;
        public bool Active;
        public int MoveCD;
        public int playerPos;
        public int upcomingPos;
        public int Wait;
        public int Time;
        public int moveDelay;
        public int MaxTime = 3600;
        public enum Direction
        {
            Left, Right, Up, Down
        }

        public override void PreUpdate()
        {
            MaxTime = 65 * 60;

            if (Main.expertMode)
                MaxTime = 60 * 60;

            if (Main.masterMode)
                MaxTime = 55 * 60;

            if (!NPC.AnyNPCs(ModContent.NPCType<ProjectNucleus>()))
                Active = false;

            if (!Active && Player.GetModPlayer<TranscendencePlayer>().NucleusDeathAnim == 0)
            {
                Time = MaxTime;
                room = 0;
                return;
            }


            if (moveDelay > 0)
                moveDelay--;
            if (moveDelay == 1)
            {
                playerPos = upcomingPos;
                SoundEngine.PlaySound(SoundID.Dig with { MaxInstances = 0 });

                if (pixel[upcomingPos] == 4)
                {
                    if (room == 7)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC n = Main.npc[i];
                            if (n != null && n.active && n.ModNPC is ProjectNucleus boss)
                            {
                                Player.velocity = new Vector2(20f, 5f);
                                n.ai[1] = 99;
                                boss.Timer_AI = 0;
                                boss.ProjectileCD = 0;
                            }
                        }
                        Active = false;
                    }
                    else ConstructLevels();
                }
            }

            if (room == 0)
                ConstructLevels();


            if (MoveCD > 0)
                MoveCD--;

            if (Wait > 0)
            {
                Wait--;
                return;
            }


            if (room > 1 && Time > 0)
                Time--;

            if ((pixel[playerPos] == 3 || Time < 1) && NPC.AnyNPCs(ModContent.NPCType<ProjectNucleus>()))
                GameFail();


            if (Player.controlLeft)
                Move(Direction.Left);

            if (Player.controlRight)
                Move(Direction.Right);

            if (Player.controlUp)
                Move(Direction.Up);

            if (Player.controlDown)
                Move(Direction.Down);
        }

        public void Move(Direction dir)
        {
            if (MoveCD > 0 || Player.dead)
                return;

            int targetPos = 0;

            if (dir == Direction.Left)
                targetPos = playerPos - 1;

            if (dir == Direction.Right)
                targetPos = playerPos + 1;

            if (dir == Direction.Up)
                targetPos = playerPos - 16;

            if (dir == Direction.Down)
                targetPos = playerPos + 16;


            if (pixel[targetPos] == 2)
                return;
            if (pixel[targetPos] == 3)
                Wait = 45;

            upcomingPos = targetPos;
            moveDelay = 5;
            MoveCD = 10;
        }

        public void GameFail()
        {
            Player.KillMe(PlayerDeathReason.ByCustomReason(Language.GetTextValue("Mods.TranscendenceMod.Messages.Death.NucleusGame", Player.name)), 9999, 1);
        }

        public void ConstructLevels()
        {
            room += 1;
            MoveCD = 30;

            switch (room)
            {
                case 0: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 1: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 0, 2, 3, 3, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 1, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 3, 3, 2, 2, 0, 3, 0, 2, 2,
                2, 3, 3, 3, 3, 3, 2, 3, 3, 2, 2, 0, 3, 0, 2, 2,
                2, 0, 0, 0, 0, 0, 2, 3, 3, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 4, 0, 0, 0, 2, 3, 3, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 2: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 2, 2, 2, 3, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 0, 0, 0, 3, 2,
                2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 3, 0, 2, 0, 3, 2,
                2, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 3, 2,
                2, 3, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 3, 2,
                2, 2, 2, 0, 2, 3, 3, 3, 3, 3, 3, 3, 2, 0, 3, 2,
                2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2,
                2, 3, 3, 0, 0, 0, 0, 0, 0, 4, 0, 2, 0, 1, 0, 2,
                2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 3: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 3, 2, 2, 3, 2,
                2, 2, 2, 2, 2, 2, 3, 2, 2, 3, 2, 3, 3, 3, 3, 2,
                2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 3, 2,
                2, 2, 0, 0, 2, 2, 0, 2, 2, 0, 2, 3, 0, 0, 3, 2,
                2, 0, 0, 0, 0, 2, 0, 2, 2, 0, 2, 3, 0, 0, 3, 2,
                2, 0, 3, 3, 3, 2, 1, 2, 2, 4, 2, 3, 0, 0, 3, 2,
                2, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 2,
                2, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 3, 3, 3, 3, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2,
                2, 3, 3, 3, 3, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 4: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 3, 0, 2, 0, 3, 3, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 2, 3, 0, 3, 0, 2, 0, 3, 3, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 0, 2, 0, 2, 2, 2, 2, 3, 0, 3, 2,
                2, 0, 3, 2, 2, 0, 2, 0, 2, 0, 3, 0, 3, 0, 3, 2,
                2, 0, 0, 0, 2, 0, 2, 0, 2, 0, 0, 4, 3, 0, 3, 2,
                2, 2, 3, 0, 2, 0, 2, 0, 2, 0, 3, 0, 3, 0, 3, 2,
                2, 0, 0, 0, 2, 0, 3, 0, 2, 0, 3, 3, 3, 0, 0, 2,
                2, 0, 1, 0, 2, 0, 0, 0, 2, 0, 0, 0, 3, 3, 0, 2,
                2, 0, 0, 0, 2, 0, 3, 0, 2, 3, 3, 0, 0, 0, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 5: pixel = new int[]
                {
                2, 2, 2, 2, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 2, 2, 2, 2, 2,
                2, 0, 3, 3, 3, 3, 0, 0, 0, 0, 3, 3, 3, 3, 2, 2,
                2, 0, 0, 3, 2, 3, 0, 0, 0, 0, 0, 0, 0, 3, 2, 2,
                2, 3, 4, 3, 2, 3, 3, 3, 3, 3, 3, 3, 0, 3, 2, 2,
                2, 3, 0, 3, 2, 3, 3, 3, 3, 3, 2, 3, 0, 3, 2, 2,
                2, 2, 2, 2, 2, 3, 0, 0, 0, 3, 2, 3, 0, 3, 2, 2,
                2, 2, 2, 2, 2, 3, 0, 3, 0, 3, 2, 3, 0, 3, 2, 2,
                2, 3, 0, 3, 3, 3, 0, 3, 0, 3, 2, 3, 0, 3, 2, 2,
                2, 0, 0, 0, 0, 0, 0, 3, 0, 3, 3, 3, 0, 3, 2, 2,
                2, 0, 1, 0, 2, 3, 3, 2, 0, 0, 0, 0, 0, 3, 2, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 2, 2
                }; break;

                case 6: pixel = new int[]
                {
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                2, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2,
                2, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2,
                2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
                2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 2,
                2, 0, 0, 0, 0, 0, 0, 2, 2, 2, 3, 0, 0, 0, 0, 2,
                2, 0, 4, 0, 3, 3, 0, 2, 2, 2, 0, 0, 0, 0, 0, 2,
                2, 0, 0, 0, 2, 2, 0, 3, 3, 3, 0, 0, 0, 3, 3, 2,
                2, 3, 0, 3, 2, 2, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2,
                2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2
                }; break;

                case 7: pixel = new int[]
                {
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
                3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3,
                3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3,
                3, 0, 0, 0, 2, 2, 0, 0, 0, 0, 3, 3, 0, 0, 0, 3,
                3, 0, 0, 2, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 3,
                3, 0, 0, 2, 0, 2, 2, 0, 0, 3, 0, 3, 3, 0, 0, 3,
                3, 0, 0, 2, 0, 1, 2, 0, 0, 3, 0, 4, 3, 0, 0, 3,
                3, 0, 0, 0, 2, 2, 0, 0, 0, 0, 3, 3, 0, 0, 0, 3,
                3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3,
                3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3,
                3, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3
                }; break;
            }
            for (int i = 0; i < pixel.Length; i++)
            {
                if (pixel[i] == 1)
                {
                    playerPos = i;
                    upcomingPos = i;
                    break;
                }
            }
        }
    }

    public class HeartGameDrawingSystem : ModSystem
    {
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            Player p = Main.LocalPlayer;

            if (p != null && p.active && p.TryGetModPlayer(out NucleusGame game) && game != null && (game.Active || p.GetModPlayer<TranscendencePlayer>().NucleusDeathAnim > 0))
            {
                int xOff = 0;
                int yOff = 0;
                int oFA = 0;

                int pixelSize = 32;

                int[] overflow = new int[16];

                var crt = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/CRTshader", AssetRequestMode.ImmediateLoad).Value;
                crt.Parameters["uOpacity"].SetValue(Main.rand.NextFloat(50f, 151f));

                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);

                TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, crt);


                int intensity = game.Time < 180 ? 16 : (int)MathHelper.Lerp(4, 1, game.Time / (float)game.MaxTime);
                int x = Main.screenWidth / 2 - (pixelSize * 8);
                int y = Main.screenHeight / 2 - (pixelSize * 8);

                Vector2 shake = Main.rand.NextVector2Circular(intensity, intensity);
                x += (int)shake.X;
                y += (int)shake.Y;

                if (game.Time != -1 && game.Time > 30)
                {
                    for (int i = 0; i < game.pixel.Length; i++)
                    {
                        Texture2D sprite = TextureAssets.BlackTile.Value;
                        Color col = Color.DarkRed;


                        if (game.pixel[i] == 2)
                        {
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Tile").Value;
                        }
                        if (game.pixel[i] == 3)
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Spike").Value;

                        if (game.pixel[i] == 4)
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Exit").Value;

                        float rot = 0f;
                        if (game.upcomingPos == i)
                        {
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Upcoming").Value;

                            if (game.upcomingPos == game.playerPos - 1)
                                rot = MathHelper.Pi + MathHelper.PiOver2;
                            if (game.upcomingPos == game.playerPos + 1)
                                rot = MathHelper.PiOver2;
                            if (game.upcomingPos > game.playerPos + 1)
                                rot = MathHelper.Pi;
                        }

                        if (game.playerPos == i)
                        {
                            sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Player").Value;
                            if (game.pixel[i] == 3)
                                sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/NucleusGame/Dead").Value;
                        }

                        if (game.pixel[i] > 1 || game.playerPos == i || game.upcomingPos == i)
                        {
                            spriteBatch.Draw(sprite, new Rectangle(x + xOff - 2, y + yOff, pixelSize, pixelSize), null, Color.Blue, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0f);
                            spriteBatch.Draw(sprite, new Rectangle(x + xOff + 2, y + yOff, pixelSize, pixelSize), null, Color.Lime, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0f);
                            spriteBatch.Draw(sprite, new Rectangle(x + xOff, y + yOff, pixelSize, pixelSize), null, col, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0f);
                        }
                        xOff += pixelSize;
                        overflow[oFA] += 1;

                        //Change row
                        if (overflow[oFA] >= 16)
                        {
                            oFA += 1;
                            yOff += pixelSize;
                            xOff = 0;
                        }
                    }


                    Texture2D sIMG = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/CRTshader").Value;

                    var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
                    eff.Parameters["uImageSize0"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
                    eff.Parameters["uImageSize1"].SetValue(sIMG.Size() * 4f);
                    eff.Parameters["uTime"].SetValue(0);
                    eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly / 0.2f);
                    eff.Parameters["useAlpha"].SetValue(true);
                    eff.Parameters["alpha"].SetValue(0.75f);
                    eff.Parameters["useExtraCol"].SetValue(false);
                    Main.instance.GraphicsDevice.Textures[1] = sIMG;

                    TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, eff);

                    spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

                    TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
                }


                TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);

                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Red * (0.125f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.075f));


                if (p.dead)
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);


                Texture2D nuc = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Nucleus/ProjectNucleus").Value;
                Texture2D nuc2 = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Nucleus/ProjectNucleus_Glow_Red").Value;
                Color glowCol = Color.White;
                if (game.Time < 450)
                {
                    nuc2 = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Nucleus/ProjectNucleus_Glow_Grey").Value;
                    glowCol = new Color(20, 20, 20);
                }

                spriteBatch.Draw(nuc, new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f - 670) + shake, null, Color.Lerp(Color.White, new Color(1f, 0.5f, 0.5f), (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f)), 0f, nuc.Size() * 0.5f, 14f, SpriteEffects.None, 0f);
                spriteBatch.Draw(nuc2, new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f - 670) + shake, null, glowCol, 0f, nuc2.Size() * 0.5f, 14f, SpriteEffects.None, 0f);

                DynamicSpriteFont font = FontAssets.MouseText.Value;
                if (OperatingSystem.IsWindows())
                    font = ModContent.Request<DynamicSpriteFont>(TranscendenceMod.ASSET_PATH + "/PixelatedFont", AssetRequestMode.ImmediateLoad).Value;

                string room = game.room.ToString() + "/" + "7 ROOM";
                string time = (game.Time / 60).ToString() + " TIME";
                Color flashCol = new Color(Main.masterColor, Main.masterColor / 4f, 0f);

                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)(Main.screenWidth / 2f - 120) - 2, y + 398, 244, game.room > 1 ? 104 : 64), flashCol);
                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)(Main.screenWidth / 2f - 120), y + 400, 240, game.room > 1 ? 100 : 60), Color.Black);

                if (game.room > 0)
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, room, new Vector2(Main.screenWidth / 2f - font.MeasureString(room).X / 2f, y + 420) + shake, Color.Red, 0, Vector2.Zero, Vector2.One, -1, 1f);


                if (game.room > 1)
                {
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, time, new Vector2(Main.screenWidth / 2f - font.MeasureString(time).X / 2f, y + 450) + shake, Color.Red, 0, Vector2.Zero, Vector2.One, -1, 1f);
                }

                float boom = game.Time < 30 ? MathHelper.Lerp(1f, 0f, game.Time / 30f) : 0f;
                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * boom);

            }

            base.PostDrawInterface(spriteBatch);
        }
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            base.ModifyTransformMatrix(ref Transform);

            if (Main.LocalPlayer != null && Main.LocalPlayer.active && Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer game) && game != null && game.NucleusConsumed > 0)
            {
                Transform.Zoom *= 1f + game.NucleusZoom * 8f;
            }

        }
    }
}