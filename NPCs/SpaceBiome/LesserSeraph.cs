using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles.NPCs;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    [AutoloadBossHead]
    public class LesserSeraph : SpaceBiomeNPC
    {
        public int AttackDelay;
        public float ChaseSpeed = 10;
        public Vector2 desiredPos;
        Player player;
        public int Attack;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 103725;
            NPC.defense = 12;
            NPC.damage = 85;
            NPC.knockBackResist = 0f;

            NPC.width = 228;
            NPC.height = 128;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(gold: 10, silver: 25);
            SpawnModBiomes = new int[2] { ModContent.GetInstance<CosmicDimensions>().Type,
                ModContent.GetInstance<Heaven>().Type };
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;
        public override bool CanHitNPC(NPC target) => false;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<QuantumSlicer>(), 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulverizedPlanet>(), 1, 14, 18));
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)    
        {
            NPC.life = (int)(NPC.life * 0.525f);
            NPC.lifeMax = (int)(NPC.lifeMax * 0.55f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            player = Main.player[NPC.target];
            NPC.takenDamageMultiplier = 1f;

            NPC.velocity *= 0.9f;

            switch (Attack)
            {
                case 0: Homing(); break;
                case 1: TeleportSpam(); break;
                case 2: Deathrays(); break;
                case 3: Lanes(); break;
                case 4: BirthOfAStar(); break;
                case 5: Attack = 0; goto case 0;
            }

            void TeleportSpam()
            {
                if (++AttackDelay % 10 == 0)
                {
                    Teleport(player.Center + Vector2.One.RotatedByRandom(360) * Main.rand.NextFloat(400, 450));
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 pos = NPC.Center + Vector2.One.RotatedBy(NPC.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver4) * (i * 10);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, NPC.DirectionTo(player.Center) * 5, ModContent.ProjectileType<HolyFireball>(), 85, 1f);

                        Vector2 pos2 = NPC.Center + Vector2.One.RotatedBy(NPC.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2 - MathHelper.PiOver4) * (i * 10);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos2, NPC.DirectionTo(player.Center) * 5, ModContent.ProjectileType<HolyFireball>(), 85, 1f);
                    }
                }
                if (AttackDelay > 60)
                {
                    Attack++;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                    AttackDelay = 0;
                }
            }

            void Teleport(Vector2 pos)
            {
                SoundEngine.PlaySound(ModSoundstyles.SeraphTeleport with
                {
                    Volume = 0.3f,
                    MaxInstances = 0
                }, player.Center);
                NPC.Center = pos;
            }

            void Homing()
            {
                if (++AttackDelay < 45 && AttackDelay % 1 == 0)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -(float)Math.Sin(AttackDelay / 5f)).RotatedBy(MathHelper.ToRadians(AttackDelay * 4 + (float)Math.Sin(AttackDelay) * 5)) * 2, ModContent.ProjectileType<HolyBolt>(), 80, 1f, -1, 1, 0, 60);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -(float)Math.Sin(AttackDelay / 5f)).RotatedBy(MathHelper.ToRadians(-AttackDelay * 4 + (float)Math.Sin(AttackDelay) * 5)) * 2, ModContent.ProjectileType<HolyBolt>(), 80, 1f, -1, 1, 0, 60);
                }
                if (AttackDelay > 180)
                {
                    Attack++;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                    AttackDelay = 0;
                }
            }

            void Deathrays()
            {
                if (++NPC.ai[2] < 30)
                    return;

                AttackDelay++;
                NPC.ai[3]++;

                if (AttackDelay % 5 == 0 && NPC.ai[3] < 45)
                {
                    for (int i = 0; i < 8; i++)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center).RotatedByRandom(0.2f) * Main.rand.NextFloat(7f, 12f), ModContent.ProjectileType<HolyFireball>(), 85, 1f);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center), ModContent.ProjectileType<AngelicLaser>(), 85, 1f);

                    NPC.velocity = NPC.DirectionTo(player.Center) * -4f;
                }
                if (NPC.ai[3] > 75)
                {
                    NPC.velocity = NPC.DirectionTo(player.Center) * 6f;

                    if (NPC.ai[3] > 150)
                        NPC.ai[3] = 0;
                }
                if (AttackDelay > 451)
                {
                    Attack++;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                    AttackDelay = 0;
                }
            }

            void Lanes()
            {
                if (++NPC.ai[2] < 45)
                {
                    return;
                }

                if (++AttackDelay % 2 == 0 && AttackDelay < 150)
                {
                    for (int i = 1; i < 3; i++)
                    {
                        TranscendenceUtils.ProjectileShotgun(new Vector2(0, 7.25f + (i / 4f)).RotatedBy(MathHelper.ToRadians(AttackDelay * 12f)), NPC.Center, NPC.GetSource_FromAI(), ModContent.ProjectileType<HolyFireball>(), 80, 2, 1, 1, 20, -1, 0, NPC.whoAmI, 0, 0);
                        TranscendenceUtils.ProjectileShotgun(new Vector2(0, -7.25f - (i / 4f)).RotatedBy(MathHelper.ToRadians(AttackDelay *12f)), NPC.Center, NPC.GetSource_FromAI(), ModContent.ProjectileType<HolyFireball>(), 80, 2, 1, 1, 20, -1, 0, NPC.whoAmI, 0, 0);
                    }
                }
                if (AttackDelay > 180)
                {
                    Attack++;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                    AttackDelay = 0;
                }
            }

            void BirthOfAStar()
            {
                int BoundarySize = 600;

                if (AttackDelay == 1)
                    Teleport(player.Center - new Vector2(0, 375));

                for (int i = 0; i < 148; i++)
                {
                    Vector2 pos = NPC.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 148 + TranscendenceWorld.UniversalRotation) * BoundarySize;
                    int d = Dust.NewDust(pos, 1, 1, ModContent.DustType<ArenaDust>(), 0, 0, 0, Color.Gold, 0.75f);
                    Main.dust[d].velocity = Vector2.Zero;
                }

                if (++AttackDelay < 60)
                    return;

                if (player.Distance(NPC.Center) > (BoundarySize * 1.35f))
                {
                    player.Center = player.Center.MoveTowards(NPC.Center, 15f);
                    NPC.takenDamageMultiplier = 0f;
                    return;
                }

                if (AttackDelay % (12 * NPC.CountNPCS(Type)) == 0)
                {
                    Vector2 pos = NPC.Center + Vector2.One.RotatedBy(NPC.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver4) * ((BoundarySize + 60) * 1.1f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, pos.DirectionTo(NPC.Center) * 6,
                        ModContent.ProjectileType<HolyFireball>(), 80, 2);
                }
                if (AttackDelay == 280 || AttackDelay == 90)
                {
                    TranscendenceUtils.ProjectileRing(NPC, 8, NPC.GetSource_FromAI(), NPC.Center, ModContent.ProjectileType<AngelicLaser>(), 75, 0.5f, 1, 0, 0, 0, -1, 0, 1);
                }
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p != null && p.active && p.type == ModContent.ProjectileType<HolyFireball>() && p.Distance(NPC.Center) < 50)
                        p.Kill();
                }
                if (AttackDelay > 460)
                {
                    Attack++;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                    AttackDelay = 0;
                }
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Scorpio")),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "TranscendenceMod/Miscannellous/Assets/Bestiary/LesserSeraph",
                PortraitScale = 0.7f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float scale = 3.5f + (float)Math.Sin(TranscendenceWorld.UniversalRotation * 1.75f) * 0.33f;

            TranscendenceUtils.DrawEntity(NPC, Color.CornflowerBlue, scale, "bloom", 0, NPC.Center, null);
            TranscendenceUtils.DrawEntity(NPC, Color.White, scale * 0.5f, "bloom", 0, NPC.Center, null);
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frame.Y != (frameHeight * 2))
            {
                if (++NPC.frameCounter > 4)
                {
                    NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0;
                }
            }
            else NPC.frame.Y = 0;
        }
    }
}

