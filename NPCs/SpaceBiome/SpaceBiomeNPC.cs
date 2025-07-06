using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    //Makes npcs in space biome spawn with Starfruits
    public abstract class SpaceBiomeNPC : ModNPC
    {
        public bool HasStarFruit = false;
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            if (Main.rand.NextBool(10) && NPC.downedMoonlord)
            {
                NPC.lifeMax *= 2;
                NPC.life *= 2;
                HasStarFruit = true;
            }
        }
        public override void OnKill()
        {
            if (HasStarFruit)
            {
                Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ModContent.ItemType<Starfruit>());
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(spriteBatch, screenPos, drawColor);
            if (HasStarFruit)
            {
                TranscendenceUtils.DrawEntity(NPC, Color.White, 1, $"TranscendenceMod/Items/Consumables/Starfruit", NPC.velocity.X * 0.02f, NPC.Center - new Vector2(0, NPC.height), null);
            }
        }
    }
}

