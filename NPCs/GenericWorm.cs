using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TranscendenceMod.NPCs
{
    public abstract class HeadSegment : ModNPC
    {
        /// <summary>
        /// How many segments the worm will have
        /// </summary>
        public abstract int MaxSegments { get; }
        /// <summary>
        /// Determines the NPCID of the Body Segment
        /// </summary>
        public abstract int BodySegmentType { get; }
        /// <summary>
        /// Determines the NPCID of the Tail Segment
        /// </summary>
        public abstract int TailSegmentType { get; }
        //The body segments break when I try to spawn them here, so spawn them in NPC AI()
        //Example in SpaceWorm_Head.cs
    }
    public abstract class BodySegment : ModNPC
    {
        public override bool CheckActive() => false;
        /// <summary>
        /// How faraway each segment will be
        /// </summary>
        public abstract float DistanceBetweenSegments { get; }
        public int DisappearTimer;
        public int BaseWidth;
        public int BaseHeight;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            BaseWidth = NPC.width;
            BaseHeight = NPC.height;
        }
        public void BodyAI(NPC npc)
        {
            void Disappear()
            {
                if (++DisappearTimer > 2)
                {
                    npc.HitEffect();
                    npc.active = false;
                }
            }

            NPC owner = Main.npc[(int)npc.ai[0]];

            if (owner == null || !owner.active)
            {
                Disappear();
                return;
            }

            FollowSegment();

            npc.scale = MathHelper.Lerp(npc.scale, owner.scale, 0.1f);

            void FollowSegment()
            {
                NPC n = null;
                if (n == null)
                {
                    if (npc.ai[1] > 0)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC nPC = Main.npc[i];
                            if (nPC != null && nPC.active && nPC.ai[1] == npc.ai[1] - 1 && (nPC.ModNPC is BodySegment || nPC.ModNPC is TailSegment) && nPC.ai[0] == npc.ai[0])
                            {
                                n = nPC;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC nPC = Main.npc[i];
                            if (nPC != null && nPC.active && nPC.ModNPC is HeadSegment && nPC.whoAmI == npc.ai[0])
                            {
                                n = nPC;
                            }
                        }
                    }
                }

                if (n == null || !n.active)
                {
                    Disappear();
                    return;
                }

                Vector2 pos = n.Center - npc.Center;

                float sq = (float)Math.Sqrt(pos.X * pos.X + pos.Y * pos.Y);
                float dis = (sq - npc.height * DistanceBetweenSegments) / sq;
                Vector2 pos2 = pos * dis;
                npc.Center += pos2;
                npc.rotation = npc.DirectionTo(n.Center).ToRotation() + MathHelper.PiOver2;
                npc.velocity = Vector2.Zero;
            }
        }
    }
    public abstract class TailSegment : BodySegment
    {
    }
}

