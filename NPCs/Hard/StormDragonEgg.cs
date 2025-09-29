using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Projectiles.NPCs.Bosses.Dragon;

namespace TranscendenceMod.NPCs.Hard
{
    public class StormDragonEgg : ModNPC
    {
        public int AttackDelay;
        public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 2;

        public override void SetDefaults()
        {
            NPC.lifeMax = 832;
            NPC.defense = 15;
            NPC.damage = 85;

            NPC.width = 102;
            NPC.height = 48;
            NPC.noGravity = true;
            NPC.noTileCollide = false;

            NPC.HitSound = SoundID.DD2_CrystalCartImpact;
            NPC.DeathSound = SoundID.NPCDeath18;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(0, 0, 10);

            NPC.aiStyle = NPCAIStyleID.AncientVision;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunburntAlloy>(), 5, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.FriedEgg, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.Cloud, 2, 3, 17));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.hardMode)
                return 0f;

            if (Main.LocalPlayer.ZoneSkyHeight && !Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ZoneStar)
            {
                return SpawnCondition.Sky.Chance * 0.5f;
            }
            return SpawnCondition.OverworldDayRain.Chance * 0.7f;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            int projCD = 30;
            if (++AttackDelay > projCD)
            {
                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(25, 0), NPC.DirectionTo(player.Center) * 10,
                    ModContent.ProjectileType<StrongWater>(), 60, 0);
                Main.projectile[p].extraUpdates = 0;

                int p2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(25, 0), NPC.DirectionTo(player.Center) * 10,
                    ModContent.ProjectileType<StrongWater>(), 60, 0);
                Main.projectile[p2].extraUpdates = 0;
                AttackDelay = 0;
            }
        }
        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            NPC.velocity = NPC.DirectionTo(-NPC.velocity) * hit.Knockback * 15;
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            NPC.velocity = NPC.DirectionTo(-NPC.velocity) * hit.Knockback * 15;
        }
        public override void FindFrame(int frameHeight)
        {
            if (++NPC.frameCounter > 6)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;

                if (NPC.frame.Y > frameHeight)
                    NPC.frame.Y = 0;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.WindyDay,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.AtmospheragonEgg")),
            });
        }
        public override bool? CanFallThroughPlatforms() => true;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
    }
}

