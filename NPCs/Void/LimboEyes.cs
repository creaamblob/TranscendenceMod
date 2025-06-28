using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles.NPCs;

namespace TranscendenceMod.NPCs.Void
{
    public class LimboEyes : ModNPC
    {
        public int AttackDelay;
        public float ChaseSpeed = 10;
        public Vector2 desiredPos;
        Player player;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 3255;
            NPC.defense = 20;
            NPC.damage = 85;
            NPC.knockBackResist = 0f;

            NPC.width = 74;
            NPC.height = 58;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 50);
            SpawnModBiomes = new int[2] { ModContent.GetInstance<CosmicDimensions>().Type,
                ModContent.GetInstance<Limbo>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidFragment>(), 1, 2, 3));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneLimbo && NPC.CountNPCS(Type) < 2)
                return 0.45f;
            else return 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)    
        {
            NPC.damage = (int)(NPC.damage * 0.525f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            player = Main.player[NPC.target];

            if (++AttackDelay > 50 && AttackDelay < 90)
            {
                NPC.ai[2] = player.Center.X > NPC.Center.X ? 1 : -1;
                NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center - new Vector2(NPC.ai[2] * 400f, 400)) * 24f, 0.1f);
            }
            if (AttackDelay > 110)
            {
                NPC.velocity.X = 10f * NPC.ai[2];
                NPC.velocity.Y *= 0.9f;

                float speed = 5f;
                if (NPC.Center.Y > player.Center.Y)
                    speed = -speed;

                if (AttackDelay % 5 == 0)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Bottom, new Vector2(0, speed), ModContent.ProjectileType<TwistedTendril>(), 80, 0);
            }
            if (AttackDelay > 200)
            {
                NPC.velocity.X = 0;
                AttackDelay = 0;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.LimboEyes")),
            });
        }
    }
}

