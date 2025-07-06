using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.NPCs.SpaceBiome.Worm
{
    public class SpaceWorm_Head : HeadSegment
    {
        public override int MaxSegments => 20;
        public int Segments2;
        public float rot;
        public int Timer;
        public float speed;

        public override int BodySegmentType => ModContent.NPCType<SpaceWorm_Body>();

        public override int TailSegmentType => ModContent.NPCType<SpaceWorm_Tail>();

        public override void OnSpawn(IEntitySource source)
        {
            //SpawnSegments(NPC);
            NPC.GetGlobalNPC<TranscendenceNPC>().Worm = Main.rand.Next(0, int.MaxValue - 1);
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Daybreak] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPC.buffImmune[ModContent.BuffType<SpaceDebuff>()] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 67 * 1000;
            NPC.defense = 0;
            NPC.damage = 85;
            NPC.knockBackResist = 0;

            NPC.width = 32;
            NPC.height = 32;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.rarity = 4;

            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(gold: 2, silver: 50);
            SpawnModBiomes = new int[2] { ModContent.GetInstance<CosmicDimensions>().Type,
                ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulverizedPlanet>(), 1, 3, 7));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar && NPC.downedMoonlord && NPC.CountNPCS(Type) < 2)
                return 0.175f;
            else return 0;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.525f);
            NPC.damage = (int)(NPC.damage * 0.525f);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            speed += 0.125f;
            if (speed > 60)
                speed = 0;

            float speed2 = (float)Math.Sin(speed) * 60;

            NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center + Vector2.One.RotatedBy(rot * (float)Math.Tan(rot / 20)) * 100) * (50 + (speed2 * 0.75f)), 0.005585f);
            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
            NPC npc = NPC;
            rot += 0.033f;

            if (++Timer > 90)
            {
                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center + player.velocity * 100) * 5.75f, ModContent.ProjectileType<GrowingOrb>(), 80, 2, -1, 0, 0, 0);
                Main.projectile[p].GetGlobalProjectile<TranscendenceProjectiles>().SpaceBossPortalProjectile = 6;
                Timer = 0;
            }

            if (Segments2 < MaxSegments)
            {
                int n = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, BodySegmentType, npc.whoAmI);
                Main.npc[n].ai[0] = npc.whoAmI;
                Main.npc[n].ai[1] = Segments2;
                Main.npc[n].ai[2] = MaxSegments;
                Main.npc[n].realLife = npc.whoAmI;

                if (Segments2 == MaxSegments - 1)
                {
                    int n2 = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, TailSegmentType, npc.whoAmI);
                    Main.npc[n2].ai[0] = npc.whoAmI;
                    Main.npc[n2].ai[1] = MaxSegments;
                    Main.npc[n2].realLife = npc.whoAmI;
                }
                Segments2++;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.EtherealScourge")),
            });
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                CustomTexturePath = "TranscendenceMod/Miscannellous/Assets/Bestiary/EtherealScourge",
                PortraitScale = 0.66f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[NPC.target];
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/CelestialSeraphTelegraph").Value;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            float size = MathHelper.Lerp(0f, 3f, (Timer - 60) / 30f);
            float dir = NPC.DirectionTo(player.Center + player.velocity * 100).ToRotation();

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(NPC, Color.OrangeRed * size, 2f, "TranscendenceMod/Miscannellous/Assets/BloomLine", dir - MathHelper.Pi - MathHelper.PiOver2, NPC.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, Texture + "_Glow", NPC.rotation, NPC.Center, null);
            if (Timer > 60)
            {
                TranscendenceUtils.DrawEntity(NPC, Color.Orange, size, "bloom", NPC.rotation, NPC.Center, null);
                TranscendenceUtils.DrawEntity(NPC, Color.White, size * 0.25f, "bloom", NPC.rotation, NPC.Center, null);
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return (int)NPC.ai[1] == (int)(NPC.ai[2] / 2);
        }
    }
    public class SpaceWorm_Body : BodySegment
    {
        public override float DistanceBetweenSegments => 1f;

        public override void AI()

        {
            BodyAI(NPC);
        }
        public override void SetStaticDefaults()
        {
            TranscendenceUtils.BeGoneBestiary(NPC);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Daybreak] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPC.buffImmune[ModContent.BuffType<SpaceDebuff>()] = true;
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 257 * 1000;
            NPC.defense = 0;
            NPC.damage = 85;
            NPC.knockBackResist = 0;

            NPC.width = 44;
            NPC.height = 28;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(gold: 5);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            string sprite = NPC.ai[1] % 2 == 0 ? Texture + "2" : Texture;
            TranscendenceUtils.DrawEntity(NPC, drawColor, NPC.scale, sprite, NPC.rotation, NPC.Center, null);
            TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, sprite + "_Glow", NPC.rotation, NPC.Center, null);

            if (Main.npc[(int)NPC.ai[0]].ModNPC is SpaceWorm_Head head && head.Timer > 60)
            {
                TranscendenceUtils.DrawEntity(NPC, Color.Orange * 0.5f, MathHelper.Lerp(0f, 3f, (head.Timer - 60) / 30f), "bloom", NPC.rotation, NPC.Center, null);
                TranscendenceUtils.DrawEntity(NPC, Color.White * 0.5f, MathHelper.Lerp(0f, 3f, (head.Timer - 60) / 30f) * 0.25f, "bloom", NPC.rotation, NPC.Center, null);
            }

            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
    }
    public class SpaceWorm_Tail : TailSegment
    {
        public override float DistanceBetweenSegments => 0.75f;
        public override void SetStaticDefaults()
        {
            TranscendenceUtils.BeGoneBestiary(NPC);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Daybreak] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPC.buffImmune[ModContent.BuffType<SpaceDebuff>()] = true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            TranscendenceUtils.DrawEntity(NPC, Color.White, NPC.scale, Texture + "_Glow", NPC.rotation, NPC.Center, null);
        }
        public override void AI()
        {
            BodyAI(NPC);
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 257 * 1000;
            NPC.defense = 0;
            NPC.damage = 85;
            NPC.knockBackResist = 0;

            NPC.width = 24;
            NPC.height = 50;

            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath22;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(gold: 5);
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;
    }
}

