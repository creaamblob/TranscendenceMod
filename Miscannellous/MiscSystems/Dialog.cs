using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;
using System;
using Terraria.UI;
using System.Collections.Generic;
using Terraria.UI.Chat;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.ID;

namespace TranscendenceMod
{
    public class DialogManagerer : ModSystem
    {
        public static int MaxDialog => 250;

        internal DialogUI state;
        private UserInterface state2;

        public override void Load()
        {
            base.Load();

            state = new DialogUI();
            state.Activate();

            state2 = new UserInterface();
            state2.SetState(state);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            state2?.Update(gameTime);
        }

        public override void PostUpdateWorld()
        {
            base.PostUpdateWorld();

            if (DialogUI.Dialog == null)
                return;

            if (DialogUI.Dialog.Count < 1)
                return;

            for (int i = 0; i < DialogUI.Dialog.Count; i++)
            {
                Dialog dialog = DialogUI.Dialog[i];
                dialog.Update();
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: Dialogue", delegate
                {
                    state2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
    public class DialogUI : UIState
    {
        public static List<Dialog> Dialog = new List<Dialog>();
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Dialog.Count == 0)
                return;

            for (int i = 0; i < Dialog.Count; i++)
            {
                Dialog dialog = Dialog[i];
                dialog.Update();
            }
        }

        public static Asset<DynamicSpriteFont> font = ModContent.Request<DynamicSpriteFont>(TranscendenceMod.ASSET_PATH + "/AncientRuneFont", AssetRequestMode.ImmediateLoad);

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Dialog.Count < 1)
                return;

            DynamicSpriteFont font2 = FontAssets.MouseText.Value;
            if (OperatingSystem.IsWindows())
                font2 = font.Value;

            for (int i = 0; i < Dialog.Count; i++)
            {
                if (font2 != null && Dialog[i].UnfinishedSentence != null && Dialog[i].UnfinishedSentence != "" && Dialog[i].UnfinishedSentence.Length > 0)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    Dialog dialog = Dialog[i];

                    float x = font2.MeasureString(Dialog[i].UnfinishedSentence).X / 2f;
                    float a =
                        // Only start fading out after half of the time has passed
                        dialog.TimeLeft < (dialog.MaxTimeLeft / 2f) ? 1f :
                        // The Fadeout Effect
                        ((dialog.MaxTimeLeft - dialog.TimeLeft) / (float)dialog.MaxTimeLeft) * 2f;

                    Vector2 pos0 = dialog.Center;
                    if (dialog.Anchor != null && dialog.Anchor.active)
                        pos0 = dialog.Anchor.Center - Main.screenPosition + dialog.Center;

                    Vector2 pos = pos0 - new Vector2(x, 0);
                    Color defCol = new Color(63, 65, 151, 255) * 0.785f;

                    if (dialog.ScreenLock)
                    {
                        pos = dialog.Center - new Vector2(x, 0);
                        Rectangle boxRec = new Rectangle((int)pos.X - 10, (int)pos.Y - 5, (int)x * 2 + 20, 40);

                        if (a > 0.1f)
                            Utils.DrawInvBG(spriteBatch, boxRec, defCol * a);
                        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font2, dialog.UnfinishedSentence, pos, dialog.Color * a, 0f, Vector2.Zero, Vector2.One);
                    }
                    else
                    {
                        if (dialog.boxType != DialogBoxes.None)
                        {
                            Rectangle lRec = new Rectangle(0, 0, 18, 32);
                            Rectangle lRec2 = new Rectangle((int)pos.X - 28, (int)pos.Y - 3, 18, 40);

                            Rectangle mRec = new Rectangle(19, 0, 59, 32);
                            int midOff = (int)x * 2 + 20;
                            Rectangle mRec2 = new Rectangle((int)pos.X - 10, (int)pos.Y - 3, midOff, 40);

                            Rectangle rRec = new Rectangle(78, 0, 18, 32);
                            Rectangle rRec2 = new Rectangle((int)pos.X - 10 + midOff, (int)pos.Y - 3, 18, 40);

                            string spritepath = $"TranscendenceMod/Miscannellous/UI/Dialog/{dialog.boxType}";
                            Texture2D sprite = ModContent.Request<Texture2D>(spritepath).Value;

                            spriteBatch.Draw(sprite, lRec2, lRec, Color.White * a);
                            spriteBatch.Draw(sprite, mRec2, mRec, Color.White * a);
                            spriteBatch.Draw(sprite, rRec2, rRec, Color.White * a);

                        }
                        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font2, dialog.UnfinishedSentence, pos, dialog.Color * a, 0f, Vector2.Zero, Vector2.One);
                    }

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend);
                }
            }
        }

        public static void SpawnDialog(string Sentence, Vector2 Center, int TimeLeft, Color? col)
        {
            if (Dialog.Count >= DialogManagerer.MaxDialog)
                return;

            Dialog dialog = new Dialog();

            dialog.CurrentSentence = Sentence;
            dialog.Center = Center;
            dialog.MaxTimeLeft = TimeLeft;

            if (col != null)
                dialog.Color = (Color)col;

            Dialog.Add(dialog);
        }
        internal static void SpawnDialog(string Sentence, bool ScreenLock, Vector2 offSet, int TimeLeft, Color? col)
        {
            if (Dialog.Count >= DialogManagerer.MaxDialog)
                return;

            Dialog dialog = new Dialog();

            dialog.CurrentSentence = Sentence;
            dialog.Center = offSet;
            dialog.MaxTimeLeft = TimeLeft;
            dialog.ScreenLock = ScreenLock;

            if (col != null)
                dialog.Color = (Color)col;

            Dialog.Add(dialog);
        }
        internal static void SpawnDialogCutscene(string Sentence, DialogBoxes boxType, int Progress, int ChainLength, Entity Anchor, Vector2 offSet, int TimeLeft, Color? col)
        {
            if (Dialog.Count >= DialogManagerer.MaxDialog)
                return;

            Dialog dialog = new Dialog();

            dialog.ChainRootSentence = Sentence;
            dialog.ChainSentence = Language.GetTextValue(Sentence + $".{Progress}");
            dialog.Anchor = Anchor;
            dialog.Center = offSet;
            dialog.MaxTimeLeft = TimeLeft;
            dialog.boxType = boxType;
            dialog.ChainProgress = Progress;
            dialog.ChainLength = ChainLength;
            dialog.IsADialogChain = true;

            if (col != null)
                dialog.Color = (Color)col;

            Dialog.Add(dialog);
        }

        public static void KillDialog(Dialog dialog)
        {
            if (dialog.IsADialogChain && dialog.ChainProgress < dialog.ChainLength)
            {
                SpawnDialogCutscene(dialog.ChainRootSentence, dialog.boxType, dialog.ChainProgress + 1, dialog.ChainLength, dialog.Anchor, dialog.Center, dialog.MaxTimeLeft, dialog.Color);
            }
            Dialog.Remove(dialog);
        }
    }
    public class Dialog
    {
        public string UnfinishedSentence;
        public string CurrentSentence = "Hello World!";

        public int Timer;
        public int TimeLeft;
        public int MaxTimeLeft = 45;

        public Color Color = Color.White;
        public Vector2 Center;
        public int SentenceProgress;
        public bool ScreenLock;
        public DialogBoxes boxType = DialogBoxes.None;

        public bool IsADialogChain;
        public int ChainProgress;
        public int ChainLength;
        public string ChainRootSentence = "";
        public string ChainSentence = "";
        public Entity Anchor;

        public void Update()
        {
            if (!Main.hasFocus || Main.gamePaused)
                return;

            string sentence = CurrentSentence;
            if (ChainSentence != "")
                sentence = ChainSentence;

            if (SentenceProgress >= sentence.Length)
            {
                if (TimeLeft < MaxTimeLeft)
                    TimeLeft++;
                else DialogUI.KillDialog(this);
            }
            else
            {
                if (++Timer % 5 == 0)
                {
                    if (IsADialogChain && Anchor != null && Anchor.active)
                        SoundEngine.PlaySound(SoundID.Mech with { MaxInstances = 0}, Anchor.Center + Center);

                    UnfinishedSentence += sentence[SentenceProgress];
                    SentenceProgress++;
                }
            }
        }
    }
}

