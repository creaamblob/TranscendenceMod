using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Miscannellous;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.NPCs.Passive
{
    public class SnowmanNPC : ModNPC
    {

        string ShopText = Language.GetTextValue("LegacyInterface.28");
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
            NPCID.Sets.NoTownNPCHappiness[Type] = true;
        }
        public override void SetDefaults()
        {
            /*Stats*/
            NPC.lifeMax = 750;
            NPC.defense = 15;
            NPC.damage = 35;
            NPC.width = 34;
            NPC.height = 44;
            NPC.aiStyle = -1;

            /*Colision*/
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0f;

            /*Audio*/
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = true;
            NPC.lavaImmune = false;
            TownNPCStayingHomeless = true;
        }
        public override void SaveData(TagCompound tag)
        {
            tag["Name"] = NPC.GivenName;
        }
        public override void LoadData(TagCompound tag)
        {
            NPC.GivenName = tag.GetString("Name");
        }
        public override bool CheckActive() => false;
        public override bool NeedSaving() => true;
        public override bool CanChat() => true;
        public override void AI()
        {
            NPC.TargetClosest(true);
            NPC.spriteDirection = NPC.direction;

            NPC.homeTileX = (int)(NPC.Bottom.X / 16f);
            NPC.homeTileY = (int)((NPC.Bottom.Y - 1.5f) / 16f);
        }
        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Flake",
                "Frosty",
                "Coldhead",
                "Snowy"
            };
        }
        public override bool CanTownNPCSpawn(int numTownNPCs) => false;
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = ShopText;
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            Main.npcChatText = GetChat();
            shop = ShopText;
        }
        public override void AddShops()
        {
            var npcshop = new NPCShop(Type, ShopText);
            {
                /*Always available*/
                TranscendenceUtils.sell(npcshop, ItemID.SnowBlock, Item.buyPrice(silver: 1));
                TranscendenceUtils.sell(npcshop, ItemID.IceBlock, Item.buyPrice(silver: 5));
                TranscendenceUtils.sell(npcshop, ItemID.FlinxFur, Item.buyPrice(gold: 1, silver: 75));
                TranscendenceUtils.sell(npcshop, ItemID.IceSkates, Item.buyPrice(gold: 20));
                TranscendenceUtils.sell(npcshop, ItemID.IceCream, Item.buyPrice(gold: 2, silver: 50));


                /* Locked Stuff */
                TranscendenceUtils.sell(npcshop, ItemID.Milkshake, Item.buyPrice(gold: 3, silver: 75), Condition.Hardmode);
                TranscendenceUtils.sell(npcshop, ModContent.ItemType<JollyMedallion>(), Condition.DownedPlantera);
                TranscendenceUtils.sell(npcshop, ItemID.SnowGlobe, Item.buyPrice(gold: 20), Condition.DownedQueenSlime);
            }
            npcshop.Register();
        }
        public override string GetChat()
        {
            WeightedRandom<string> dialog = new();

            if (!Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().TalkedToSnowy)
            {
                Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().TalkedToSnowy = true;
                return Language.GetTextValue($"Mods.TranscendenceMod.NPCs.SnowmanNPC.Dialogue.Intro");
            }

            dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.SnowmanNPC.Dialogue.Generic1"));
            dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.SnowmanNPC.Dialogue.Generic2"));
            dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.SnowmanNPC.Dialogue.Generic3"));

            if (!Downed.Contains(Bosses.FrostSerpent))
                dialog.Add(Language.GetTextValue($"Mods.TranscendenceMod.NPCs.SnowmanNPC.Dialogue.Serpent"));

            return dialog;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            ContentSamples.NpcBestiaryRarityStars[NPC.type] = 3;
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue($"Mods.TranscendenceMod.Messages.Bestiary.Snowman"))
            });
        }
    }
}