using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.NPCs
{
    public class DummyPro : ModNPC
    {
        public int AttackDelay;
        public int HitTimer;
        private Texture2D sprite;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            TranscendenceUtils.BeGoneBestiary(NPC);
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 1;
            NPC.damage = 55;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;

            NPC.width = 40;
            NPC.height = 48;

            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.chaseable = true;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.justHit)
                HitTimer = 5;

            if (HitTimer > 0 && NPC.frame.Y != (frameHeight * 5))
            {
                Lighting.AddLight(NPC.Center, 0.6f, 0.2f, 0.4f);
                if (++NPC.frameCounter > 5)
                {
                    NPC.frame.Y += frameHeight;
                    HitTimer--;
                    NPC.frameCounter = 0;
                }
            }
            else NPC.frame.Y = 0;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (sprite == null)
            {
                sprite = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            }
            Vector2 position = NPC.Center + new Vector2(0, 4) - Main.screenPosition;
            Rectangle frame = NPC.frame;
            Vector2 origin = frame.Size() * 0.5f;
            Color color = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
            float rotation = NPC.rotation;
            SpriteEffects se = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(sprite, position, frame, color, rotation, origin, NPC.scale, se);
        }
        public override void AI()
        {
            NPC.lifeMax = 10000000;
            NPC.life = NPC.lifeMax;
            NPC.HitSound = Main.rand.NextBool(2) ? SoundID.NPCHit15 : Main.rand.NextBool(2) ? SoundID.NPCHit16 : SoundID.NPCHit17;
        }
    }
}

