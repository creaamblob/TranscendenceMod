using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TranscendenceMod.Items.Consumables;

namespace TranscendenceMod.NPCs.Passive
{
    public class Nighthopper : ModNPC
    {
        public int AttackDelay;
        public int HitTimer;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            Main.npcCatchable[Type] = true;
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Grasshopper);
            AIType = NPCID.Grasshopper;
            AnimationType = NPCID.Grasshopper;
            NPC.catchItem = ModContent.ItemType<EnchantedHopper>();
        }
        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;
            float num1015 = Main.rand.Next(90, 111) * 0.01f;
            num1015 *= (Main.essScale + 0.5f) / 2f;
            Lighting.AddLight((int)((NPC.position.X + NPC.width / 2) / 16f), (int)((NPC.position.Y + NPC.height / 2) / 16f), 0.3f * num1015, 0.1f * num1015, 0.25f * num1015);
        }
        public override void OnKill()
        {
            base.OnKill();
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Star.starfallBoost > 3 && !Main.dayTime)
                return SpawnCondition.OverworldDay.Chance * 0.4f;
            else return 0;
        }
        public override Color? GetAlpha(Color drawColor) => new Color(250, 250, 250, 200);
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            ContentSamples.NpcBestiaryRarityStars[NPC.type] = 4;
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Grasshoppers infused with fallen star magic glow with a glittering light. Fishes are very attracted to these.")
            });
        }
    }
}

