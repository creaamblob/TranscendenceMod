using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ModLoader;
using TranscendenceMod.NPCs.Boss.Dragon;

namespace TranscendenceMod.Miscannellous.UI
{
    public class AtmospheronBossBar : ModBossBar
    {
        public int iconIndex = -1;
        public override Asset<Texture2D> GetIconTexture(ref Rectangle? frame)
        {
            if (iconIndex != -1)
                return ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Dragon/WindDragon_Head_Boss");
            return null;
        }
        public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax)
        {
            NPC npc = Main.npc[info.npcIndexToAimAt];

            if (npc.ModNPC is WindDragon boss)
            {
                if (npc == null || !npc.active || Main.player[npc.target] == null)
                    return false;

                life = npc.life;
                lifeMax = npc.lifeMax;
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

        }
    }
}


