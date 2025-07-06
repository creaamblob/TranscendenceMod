using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Miscannellous.UI
{
    public class SpaceBossHealthbar : ModBossBar
    {
        public int iconIndex = -1;
        public override Asset<Texture2D> GetIconTexture(ref Rectangle? frame)
        {
            if (iconIndex != -1)
                return ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Seraph/CelestialSeraph_Head_Boss");
            return null;
        }
        public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax)
        {
            NPC npc = Main.npc[info.npcIndexToAimAt];

            if (npc.ModNPC is CelestialSeraph boss)
            {
                if (npc == null || !npc.active || npc.ai[1] < 2 || boss.Attack == SeraphAttacks.Stare || boss.Attack == SeraphAttacks.DeathAnim)
                    return false;

                life = npc.life;
                lifeMax = npc.lifeMax;


                shieldMax = boss.ShieldLifeMax;
                shield = boss.ShieldLife;
                iconIndex = npc.GetBossHeadTextureIndex();
            }

            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, NPC npc, ref BossBarDrawParams drawParams)
        {
            return base.PreDraw(spriteBatch, npc, ref drawParams);
        }
        public override void PostDraw(SpriteBatch spriteBatch, NPC npc, BossBarDrawParams drawParams)
        {
            Texture2D barInvinc = ModContent.Request<Texture2D>(Texture + "_Invincible").Value;
            Rectangle barRec = new Rectangle((int)(drawParams.BarCenter.X - (230 * Main.UIScale)), (int)(drawParams.BarCenter.X - (12 * Main.UIScale)), (int)(460 * Main.UIScale), (int)(18 * Main.UIScale));
            drawParams.IconTexture = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Seraph/CelestialSeraph_Head_Boss").Value;

            if (npc.ModNPC is CelestialSeraph boss2)
            {
                if (npc.dontTakeDamage && boss2.Phase > 1)
                {
                    spriteBatch.Draw(barInvinc, barRec, Color.White);
                }
            }
            if (npc.ModNPC is CelestialSeraph boss)
            {
                Player player = Main.player[npc.target];
                if (boss.CurrentAttack.Length > 2 && boss.Attack != SeraphAttacks.DeathAnim && boss.Attack != SeraphAttacks.Stare && boss.Attack != SeraphAttacks.DesperationEnter && player != null && !player.dead)
                {
                    float size = boss.CurrentAttack.Length > 40 ? 0.5f : boss.CurrentAttack.Length > 30 ? 0.66f : 0.75f;
                    float yMod = boss.CurrentAttack.Length > 30 ? -2 : 0;

                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, boss.CurrentAttack,
                        drawParams.BarCenter - new Vector2(52, 32 + yMod), Color.Gold, 0, Vector2.Zero, new Vector2(size));

                }
            }
            Rectangle icon = new Rectangle((int)(drawParams.BarCenter.X - (260 * Main.UIScale)), (int)(drawParams.BarCenter.X - (20 * Main.UIScale)), drawParams.IconTexture.Width, drawParams.IconTexture.Height);
            spriteBatch.Draw(drawParams.IconTexture, icon, drawParams.IconColor);

        }

    }
}


