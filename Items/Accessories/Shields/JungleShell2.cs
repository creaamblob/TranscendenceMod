using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class JungleShell2 : ModMount
    {
        public override void UpdateEffects(Player player)
        {
            MountData.spawnDustNoGravity = true;
            MountData.buff = ModContent.BuffType<BeetleshellBuff>();
            MountData.spawnDust = DustID.Venom;

            MountData.runSpeed = 0.8f;
            MountData.jumpHeight = 4;
            MountData.jumpSpeed = 2.85f;
            MountData.acceleration = 0.5f;

            MountData.constantJump = false;
            MountData.flightTimeMax = 30;

            MountData.fatigueMax = int.MaxValue;
            MountData.fallDamage = 0f;
            MountData.blockExtraJumps = true;
            MountData.dashSpeed = 2f;

            MountData.totalFrames = 3;

            MountData.idleFrameCount = 1;
            MountData.idleFrameDelay = 12;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;

            MountData.runningFrameCount = 1;
            MountData.runningFrameStart = 0;
            MountData.runningFrameDelay = 12;

            MountData.flyingFrameCount = 2;
            MountData.flyingFrameStart = 1;
            MountData.flyingFrameDelay = 6;

            MountData.standingFrameCount = 1;
            MountData.standingFrameStart = 0;
            MountData.standingFrameDelay = 16;

            MountData.inAirFrameCount = 2;
            MountData.inAirFrameStart = 1;
            MountData.inAirFrameDelay = 6;

            MountData.swimFrameCount = 1;
            MountData.swimFrameStart = 0;
            MountData.swimFrameDelay = 8;

            MountData.xOffset = 0;
            MountData.yOffset = 2;
            MountData.playerHeadOffset = -30;

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
