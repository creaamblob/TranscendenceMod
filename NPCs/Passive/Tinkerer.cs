using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using TranscendenceMod.Miscannellous.UI.ModifierUI;

namespace TranscendenceMod.NPCs.Passive
{
    [AutoloadHead]
    public class Tinkerer : ModNPC
    {
        Player player = Main.LocalPlayer;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
            NPC.Happiness
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Love)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Love)
                .SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Nurse, AffectionLevel.Dislike);
        }
        public override void SetDefaults()
        {
            /*Stats*/
            NPC.lifeMax = 250;
            NPC.defense = 15;
            NPC.damage = 35;
            NPC.width = 34;
            NPC.height = 44;
            NPC.aiStyle = 7;

            /*Colision*/
            NPC.noGravity = false;
            NPC.noTileCollide = false;

            /*Audio*/
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = true;
            NPC.townNPC = true;
        }
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return !toKingStatue;
        }
        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Madison",
                "Lucy",
                "Cecilia",
                "Luciana",
                "Artemis"
            };
        }
        public override void AI()
        {
            if (ModifierApplierUIDrawing.Visible)
                NPC.velocity.X = 0f;
            NPC.spriteDirection = NPC.direction;
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            if (NPC.downedBoss2) return true;
            else return false;
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue(Language.GetTextValue("Mods.TranscendenceMod.Messages.ModifierUIButton"));
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton && !ModifierApplierUIDrawing.Visible)
            {
                player.GetModPlayer<TranscendencePlayer>().ModifierUINPCPos = NPC;
                player.GetModPlayer<TranscendencePlayer>().TinkererHappiness = (float)player.currentShoppingSettings.PriceAdjustment;

                ModifierApplierUIDrawing.Visible = true;

                Main.npcChatText = string.Empty;
            }
        }
        public override string GetChat()
        {
            WeightedRandom<string> dialog = new();
            dialog.Add(Language.GetTextValue("Mods.TranscendenceMod.NPCs.Tinkerer.Dialogue.Idle1"));
            dialog.Add(Language.GetTextValue("Mods.TranscendenceMod.NPCs.Tinkerer.Dialogue.Idle2"));
            dialog.Add(Language.GetTextValue("Mods.TranscendenceMod.NPCs.Tinkerer.Dialogue.Idle3"), 0.8);
            return dialog;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            ContentSamples.NpcBestiaryRarityStars[NPC.type] = 3;
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("Placeholder"),
            });
        }
    }
}