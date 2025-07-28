using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using TranscendenceMod.Items.Accessories.Offensive;
using TranscendenceMod.NPCs.Boss.FrostSerpent;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Miscannellous.UI
{
    public class BossBar : ModBossBarStyle
    {
        public override bool PreventDraw => true;
        public override void Draw(SpriteBatch spriteBatch, IBigProgressBar currentBar, BigProgressBarInfo info)
        {
            NPC npc = Main.npc[info.npcIndexToAimAt];

            if (npc == null || !npc.active)
                return;

            bool whiteListed =
                npc.type == NPCID.BrainofCthulhu ||
                npc.type == NPCID.MoonLordCore ||
                npc.type == NPCID.MoonLordHand ||
                npc.type == ModContent.NPCType<CelestialSeraph>() && npc.ai[1] > 1;

            int headIndex = npc.GetBossHeadTextureIndex();

            if (headIndex == -1 && !whiteListed)
                return;

            Texture2D bar = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/BossBar").Value;
            Texture2D hp = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/BossBarHP").Value;
            Texture2D icon = null;
            if (whiteListed)
            {
                if (npc.type == NPCID.MoonLordCore) icon = TextureAssets.Item[ItemID.MoonLordTrophy].Value;
                if (npc.type == NPCID.MoonLordHand) icon = TextureAssets.Item[ModContent.ItemType<LunarGauntlet>()].Value;
                if (npc.type == NPCID.BrainofCthulhu) icon = TextureAssets.NpcHeadBoss[23].Value;
                if (npc.type == ModContent.NPCType<CelestialSeraph>()) icon = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Seraph/CelestialSeraph_Head_Boss").Value;
            }
            else icon = TextureAssets.NpcHeadBoss[headIndex].Value;

            int x = Main.screenWidth / 2;
            x -= bar.Width / 2;
            int y = (int)(Main.screenHeight * 0.875f);
            Rectangle barRec = new Rectangle(x, y, bar.Width, bar.Height);
            spriteBatch.Draw(bar, barRec, Color.White);


            int remaining = (int)(MathHelper.Lerp(240, 0, npc.life / (float)npc.lifeMax));
            int remVal = remaining / 7;

            Color col = (npc.dontTakeDamage || npc.ModNPC is CelestialSeraph boss && boss.NPCFade < 0.5f) ? Color.Gray * 0.66f : Color.White;

            Rectangle hpRecM = new Rectangle(x + 50 + (36 - remVal), y + 12, 240 - remaining - (36 - remVal), hp.Height);
            Rectangle hpRecM2 = new Rectangle(36, 0, 240 - 36, hp.Height);
            spriteBatch.Draw(hp, hpRecM, hpRecM2, col);

            Rectangle hpRecL = new Rectangle(x + 50, y + 12, 36 - remVal, hp.Height);
            Rectangle hpRecL2 = new Rectangle(0, 0, 36 - remVal, hp.Height);
            spriteBatch.Draw(hp, hpRecL, hpRecL2, col);

            Rectangle hpRecR = new Rectangle(x + 50 + 240 - remaining, y + 12, 36 - remVal, hp.Height);
            Rectangle hpRecR2 = new Rectangle(240 + remVal, 0, 36 - remVal, hp.Height);
            spriteBatch.Draw(hp, hpRecR, hpRecR2, col);

            int iOX = icon.Width / 2;
            int iOY = icon.Height / 2;
            Rectangle iconRec = new Rectangle(x + 20 - iOX, y + 20 - iOY, icon.Width, icon.Height);
            Rectangle iconRec2 = new Rectangle(x + 24 - iOX, y + 24 - iOY, icon.Width, icon.Height);
            spriteBatch.Draw(icon, iconRec2, Color.Black * 0.66f);
            spriteBatch.Draw(icon, iconRec, Color.White);

            DynamicSpriteFont font = FontAssets.MouseText.Value;
            string hpText = npc.life + " / " + npc.lifeMax + $" ({ Math.Round(MathHelper.Lerp(0, 100, npc.life / (float)npc.lifeMax), 1)}% )";
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font,
                hpText, new Vector2(x + 163 - font.MeasureString(hpText).X * 0.375f / 1.5f, y + 12), col * 0.66f, 0, Vector2.Zero, Vector2.One * 0.75f);

            string defText = npc.defense.ToString();
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font,
                defText, new Vector2(x + 20 - font.MeasureString(defText).X * 0.375f, y + 36), Color.Lerp(Color.Lime, Color.Aqua, 0.5f), 0, Vector2.Zero, Vector2.One * 0.75f);

            string text = npc.FullName;
            if (npc.ModNPC is CelestialSeraph boss2)
            {
                text = npc.FullName + " > " + $"[C/c200ff:{boss2.CurrentAttack}]";
            }

            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font,
                text, new Vector2(x + 58, y + 36), Color.White, 0, Vector2.Zero, Vector2.One * 0.66f);
        }
    }
}


