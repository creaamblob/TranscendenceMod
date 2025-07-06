using Terraria.ModLoader;

namespace TranscendenceMod
{
    public class SeraphEncounters : ModSystem
    {
        /*public static List<TalkingSeraph.TopicType> SeraphQuestlineProgress = new List<TalkingSeraph.TopicType>();

        public override void SaveWorldData(TagCompound tag)
        {
            List<int> proglist = new List<int>();
            for (int i = 0; i < SeraphQuestlineProgress.Count; i++)
            {
                proglist.Add((int)SeraphQuestlineProgress[i]);
            }
            List<int> Progress = proglist;
            tag["SerProgress"] = Progress;
            base.SaveWorldData(tag);
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.GetList<int>("SerProgress") != null)
            {
                for (int i = 0; i < tag.GetList<int>("SerProgress").Count; i++)
                    SeraphQuestlineProgress.Add((TalkingSeraph.TopicType)tag.GetList<int>("SerProgress")[i]);
            }
            base.LoadWorldData(tag);
        }

        public override void PostUpdateWorld()
        {
            //SeraphQuestlineProgress.Clear();

            Player player = Main.LocalPlayer;
            bool flatSurface = !Collision.SolidTiles(player.Center - new Vector2(750f), 1500, 500);

            if (Main.dayTime && flatSurface && Main.rand.NextBool(1200) && player.ZoneOverworldHeight && !NPC.AnyNPCs(ModContent.NPCType<TalkingSeraph>()))
            {
                bool Legendary = Main.getGoodWorld || Main.zenithWorld;

                RegisterEncounter(player, Main.hardMode && !Legendary, 5, "Post-WoF", TalkingSeraph.TopicType.WoF);
                RegisterEncounter(player, Main.hardMode && Legendary, 9, "Post-WoF-Legendary", TalkingSeraph.TopicType.WoF);

                RegisterEncounter(player, NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3, 7, "Post-Mechs", TalkingSeraph.TopicType.Mechs);
                RegisterEncounter(player, NPC.downedEmpressOfLight, 4, "Post-EoL", TalkingSeraph.TopicType.EoL);
                RegisterEncounter(player, TranscendenceWorld.DownedOOA, 3, "Post-OOA", TalkingSeraph.TopicType.OOA);
                RegisterEncounter(player, NPC.downedAncientCultist, 3, "Post-Cult", TalkingSeraph.TopicType.Cultist);
                RegisterEncounter(player, NPC.downedMoonlord, 4, "Post-Moon", TalkingSeraph.TopicType.MoonLord);
            }
            
            base.PostUpdateWorld();
        }

        public static void RegisterEncounter(Player player, bool Condition, int Length, string Topic, TalkingSeraph.TopicType TopicID)
        {
            if (SeraphQuestlineProgress != null && SeraphQuestlineProgress.Contains(TopicID) && TopicID != TalkingSeraph.TopicType.CasualTalk || !Condition)
                return;

            int x = (int)player.Center.X + 500 * player.direction;
            int y = (int)player.Center.Y - 275;

            int n = NPC.NewNPC(player.GetSource_FromAI(), x, y, ModContent.NPCType<TalkingSeraph>());
            NPC n2 = Main.npc[n];

            if (n2.ModNPC is TalkingSeraph boss)
            {
                boss.MaxChat = Length;
                boss.Topic = Topic;
                boss.TopicID = TopicID;
            }
        }

        internal static void RegisterEncounter(Player player, bool Condition, int Length, string Topic, TalkingSeraph.TopicType TopicID, string[] parameters)
        {
            if (SeraphQuestlineProgress != null && SeraphQuestlineProgress.Contains(TopicID) && TopicID != TalkingSeraph.TopicType.CasualTalk || !Condition)
                return;

            int x = (int)player.Center.X + 500 * player.direction;
            int y = (int)player.Center.Y - 275;

            int n = NPC.NewNPC(player.GetSource_FromAI(), x, y, ModContent.NPCType<TalkingSeraph>());
            NPC n2 = Main.npc[n];

            if (n2.ModNPC is TalkingSeraph boss)
            {
                boss.MaxChat = Length;
                boss.Topic = Topic;
                boss.TopicID = TopicID;

                boss.parameters[0] = parameters[0];
                boss.parameters[1] = parameters[1];
                boss.parameters[2] = parameters[2];
                boss.parameters[3] = parameters[3];
            }
        }*/
    }
}

