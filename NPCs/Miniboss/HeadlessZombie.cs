using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.NPCs.Miniboss
{
    [AutoloadBossHead]
    public class HeadlessZombie : ModNPC
    {
        public int Timer;
        public int ProjCD;
        public int AIState;
        public int dashDir;
        public Vector2 DashPos;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[Type] = 4;
            /*Stats*/
            NPC.npcSlots = 3f;
            NPC.lifeMax = 525;
            NPC.defense = 10;
            NPC.damage = 20;
            NPC.value = Item.buyPrice(gold: 2, silver: 50);
            NPC.width = 58;
            NPC.height = 60;
            NPC.aiStyle = 1;
            NPC.rarity = 1;
            NPC.npcSlots = 7f;

            /*Colision*/
            NPC.noGravity = false;
            NPC.noTileCollide = false;

            /*Audio*/
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;

            NPC.friendly = false;
            NPC.knockBackResist = 0f;
            AnimationType = NPCID.ChatteringTeethBomb;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<LostHead>(), ItemID.ZombieArm));
        }
        public override void OnKill()
        {
            SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
            SoundEngine.PlaySound(SoundID.NPCDeath21, NPC.Center);
            for (int i = 0; i < 185; i++)
            {
                Dust.NewDustPerfect(NPC.Center, ModContent.DustType<BetterBlood>(), new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-10f, -2.5f)), 0, default, 2f);
            }
            if (!TranscendenceWorld.DownedHeadlessZombie)
                TranscendenceWorld.DownedHeadlessZombie = true;
        }
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            NPC.spriteDirection = NPC.direction;

            /*AI*/
            if (++Timer > (AIState == 1 ? 60 : 120))
            {
                NPC.velocity = Vector2.Zero;
                AIState++;
                Timer = 0;
            }
            if (AIState == 1)
            {
                NPC.velocity.X = 0;

                if (++ProjCD % 5 == 0)
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Top, new Vector2(12 - (Timer / 7f), -3), ProjectileID.Nail, 30, 1);
                    Main.projectile[p].tileCollide = false;
                    int p2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Top, new Vector2(-12 + (Timer / 7f), -3), ProjectileID.Nail, 30, 1);
                    Main.projectile[p2].tileCollide = false;
                }
            }
            if (AIState == 2)
            {
                if (Timer < 30)
                {
                    NPC.noTileCollide = true;
                    NPC.velocity = NPC.DirectionTo(player.Center - new Vector2(player.velocity.X * 7, 400)) * 17;
                }
                else
                {
                    NPC.velocity = new Vector2(0, 50);
                    if (Collision.SolidCollision(NPC.Center + new Vector2(0, 16), NPC.width, NPC.height))
                    {
                        SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.Center);

                        TranscendenceUtils.ProjectileShotgun(new Vector2(0, -15), NPC.Center, NPC.GetSource_FromAI(),
                            ProjectileID.DeerclopsRangedProjectile, 35, 3, 1, 3, 15, -1, 0, 3, 0, 0);
                        AIState = 3;
                        Timer = 0;
                    }

                }
            }
            else NPC.noTileCollide = false;
            if (AIState == 3)
            {
                if (Timer < 30)
                {
                    dashDir = player.Center.X > NPC.Center.X ? 1 : -1;
                    NPC.velocity.Y -= 0.66f;
                    return;
                }
                NPC.velocity.X = (10 * dashDir);
                if (++ProjCD > 10 && Timer > 40)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Top, new Vector2(dashDir * -2f, -7.5f), ProjectileID.BloodNautilusShot, 15, 2);
                    ProjCD = 0;
                }
            }
            if (AIState == 4)
            {
                if (++ProjCD % 20 == 0)
                    dashDir = player.Center.X > NPC.Center.X ? 1 : -1;
                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, (15 * dashDir), 0.1f);
            }
            if (AIState == 5)
            {
                AIState = 0;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.BeheadedZombie")),
            });
        }
    }
}