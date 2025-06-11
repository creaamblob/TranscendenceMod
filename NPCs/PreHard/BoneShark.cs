using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace TranscendenceMod.NPCs.PreHard
{
    public class BoneShark : ModNPC
    {
        public override void SetStaticDefaults() { }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Shark);

            Main.npcFrameCount[Type] = 4;
            AnimationType = NPCID.Shark;

            NPC.lifeMax = 175;
            NPC.defense = 25;
            NPC.damage = 65;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;

            NPC.value = Item.buyPrice(0, 0, silver: 3);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.DivingHelmet, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.GoldenKey, 66));
            npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 3, 6));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.bloodMoon && NPC.downedBoss3)
                return SpawnCondition.OceanMonster.Chance * 0.25f;
            return 0f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.BoneShark")),
            });
        }
    }
}

