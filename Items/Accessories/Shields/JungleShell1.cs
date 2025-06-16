using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class JungleShell1 : ModMount
    {
        public override void SetStaticDefaults()
        {

            MountData.spawnDustNoGravity = true;
            MountData.buff = ModContent.BuffType<TurtleShieldBuff>();
            MountData.spawnDust = DustID.Chlorophyte;
            MountData.heightBoost = -14;

            MountData.runSpeed = 1f;
            MountData.jumpHeight = 0;
            MountData.jumpSpeed = 0f;
            MountData.acceleration = 0f;

            MountData.fatigueMax = int.MaxValue;
            MountData.fallDamage = 2f;
            MountData.blockExtraJumps = true;
            MountData.dashSpeed = 1.75f;

            MountData.totalFrames = 1;

            MountData.idleFrameCount = MountData.totalFrames;
            MountData.idleFrameDelay = 12;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;

            MountData.runningFrameCount = MountData.totalFrames;
            MountData.runningFrameStart = 0;
            MountData.runningFrameDelay = 12;

            MountData.flyingFrameCount = MountData.totalFrames;
            MountData.flyingFrameStart = 0;
            MountData.flyingFrameDelay = 6;

            MountData.standingFrameCount = MountData.totalFrames;
            MountData.standingFrameStart = 0;
            MountData.standingFrameDelay = 16;

            MountData.inAirFrameCount = MountData.totalFrames;
            MountData.inAirFrameStart = 0;
            MountData.inAirFrameDelay = 6;

            MountData.swimFrameCount = MountData.totalFrames;
            MountData.swimFrameStart = 0;
            MountData.swimFrameDelay = 8;

            MountData.xOffset = 0;
            MountData.yOffset = 6;
            MountData.playerHeadOffset = -14;

            //MountData.bodyFrame = 3;
            MountData.playerXOffset = 0;

            int[] verticalOffsets = new int[MountData.totalFrames];
            for (int l = 0; l < verticalOffsets.Length; l++)
                verticalOffsets[l] = 0;

            MountData.playerYOffsets = verticalOffsets;

            if (!Main.dedServ)
            {
                MountData.textureWidth = MountData.backTexture.Width();
                MountData.textureHeight = MountData.backTexture.Height();
            }
        }
    }
}
