using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ModLoader;
using TranscendenceMod.NPCs.Boss.FrostSerpent;

namespace TranscendenceMod.Miscannellous.UI
{
    public class FrostSerpentBossBar : ModBossBar
    {
        public int iconIndex = -1;
        public override Asset<Texture2D> GetIconTexture(ref Rectangle? frame)
        {
            if (iconIndex != -1)
                return TextureAssets.NpcHeadBoss[iconIndex];
            return null;
        }
        public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax)
        {
            NPC npc = Main.npc[info.npcIndexToAimAt];

            if (!npc.active || npc.type != ModContent.NPCType<FrostSerpent_Head>())
                return false;

            life = npc.life;
            lifeMax = npc.lifeMax;

            iconIndex = npc.GetBossHeadTextureIndex();

            return npc.type == ModContent.NPCType<FrostSerpent_Head>();
        }

    }
}


