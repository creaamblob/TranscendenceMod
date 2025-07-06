using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using TranscendenceMod.NPCs.Boss.Dragon;

namespace TranscendenceMod.Miscannellous.Skies
{
    public class DragonSky : CustomSky
    {
        private bool active = false;
        private float fadeIn = 0;
        private int perlinScroll = 10;

        public override void Activate(Vector2 position, params object[] args)
        {
            active = true;
        }
        public override void Deactivate(params object[] args)
        {
            active = false;
        }
        public override bool IsActive() => fadeIn > 0f;
        public override void Update(GameTime gameTime)
        {
            if (fadeIn < 1 && active)
                fadeIn += 0.025f;
            if (!active && fadeIn > 0)
                fadeIn -= 0.025f;

        }
        public override void Reset() => active = false;
        public override float GetCloudAlpha() => 1f;
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            int npC = NPC.FindFirstNPC(ModContent.NPCType<WindDragon>());
            if (npC != -1)
            {
                NPC npc = Main.npc[npC];
                if (npc != null && npc.type == ModContent.NPCType<WindDragon>() && npc.ModNPC is WindDragon boss && boss != null)
                {
                    Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Gradient").Value;
                    Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Perlin2").Value;

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.NonPremultiplied);

                    if (maxDepth >= float.MaxValue && minDepth < float.MaxValue)
                    {
                        spriteBatch.Draw(sprite, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight / 2), Color.White * 0.75f * fadeIn);
                    }

                    if (maxDepth >= 0 && minDepth < 0)
                    {
                        float opac = boss.Phase == 3 ? 0.75f : 0.5f;
                        spriteBatch.Draw(sprite2, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), new Rectangle(perlinScroll, perlinScroll, 70, 70), Color.White * opac * fadeIn);
                        spriteBatch.Draw(sprite2, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), new Rectangle(-perlinScroll, -perlinScroll, -70, -70),
                            Color.White * opac * fadeIn);
                    }

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend);
                }
            }
        }
    }
}


