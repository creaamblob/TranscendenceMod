
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Accessories.Movement.Wings;
using TranscendenceMod.Items.Armor.Hats;
using TranscendenceMod.Items.Consumables.LootBags;
using TranscendenceMod.Items.Consumables.Placeables.Decorations;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Items.Weapons.Summoner;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.UI;
using TranscendenceMod.Projectiles.NPCs.Bosses.Dragon;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;
namespace TranscendenceMod.NPCs.Boss.Dragon
{
    [AutoloadBossHead]
    public class WindDragon : ModNPC
    {
        //Wall of Text has awoken!
        int Tornado = ModContent.ProjectileType<TornadoHostile>();
        int Water = ModContent.ProjectileType<StrongWater>();
        int Bolt = ModContent.ProjectileType<DragonBolt>();
        int BoltTele = ModContent.ProjectileType<DragonBolt_Telegraph>();
        int Wind = ModContent.ProjectileType<WindBlastHostile>();
        int Rainbow = ModContent.ProjectileType<RainbowShot>();
        int SnowProj = ModContent.ProjectileType<Snow>();
        int CloudProj = ModContent.ProjectileType<CloudProj>();
        int laser = ModContent.ProjectileType<LightningLaser>();
        int dashLaser = ModContent.ProjectileType<DashLaser>();
        int ArenaSize = 100;
        public Vector2 arenaStartPos;
        Player player;
        Player local = Main.LocalPlayer;

        static public float RotationTimer;
        static public float RotationSpeed;
        static public int DespawnTimer;
        Vector2 RotationRadians = new Vector2(-4, 0) / 25;
        public int moveTwoAngle = 0;
        Vector2 arenaCenter;
        public Vector2 dashVel;
        public Vector2 dashPos;

        //Damage Stats
        static public int TornadoDamage = 90;
        static public int ThunderDamage = 95;
        static public int WaterDamage = 80;
        static public int WindDamage = 110;
        static public int ClawDamage = 100;
        static public int SnowDamage = 80;
        public int contactDMG = 250;

        //AI
        public int Timer_AI;

        public int AttackDuration;
        public int[] ProjectileCD = new int[5];
        public int Phase;
        public bool HasArena;
        public bool Enraged;
        public string CurrentAttack = "";
        public float arenaVisualSize;
        public int Reversal = 1;
        public int StunnedFrame;
        public int StunnedFrameTimer;
        public int RandomizationTries;
        public float Phase3SkyOpacity = 0f;
        public float Opacity = 1f;
        public int RestDashCD;

        public Vector2 FlyPos;
        private void DefaultStats()
        {
            Enraged = false;
        }

        //Set Enraged stats
        private void Enrage()
        {
            Enraged = true;
        }

        public override void SetStaticDefaults()
        {
            //Main.npcFrameCount[Type] = 3;
            NPCID.Sets.TrailCacheLength[Type] = 15;
            NPCID.Sets.TrailingMode[Type] = 1;
            NPCID.Sets.MustAlwaysDraw[Type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.BetsysCurse] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Daybreak] = true;
        }
        public override void SetDefaults()
        {
            /*Stats*/
            NPC.lifeMax = 625 * 1000;
            NPC.damage = contactDMG;
            NPC.defense = 30;
            NPC.npcSlots = 8f;
            NPC.scale = 2f;
            NPC.value = Item.buyPrice(platinum: 1);
            /*Collision*/
            NPC.width = 42;
            NPC.height = 42;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            /*Audio*/
            NPC.HitSound = SoundID.DD2_BetsyHurt;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicID.OldOnesArmy;

            NPC.aiStyle = 0;
            NPC.netUpdate = true;
            NPC.boss = true;
            NPC.friendly = false;
            NPC.knockBackResist = 0f;
            NPC.BossBar = ModContent.GetInstance<AtmospheronBossBar>();
        }
        public override bool CheckActive() => false;
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (Main.masterMode ? 0.55f : 0.575f));
        }
        public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.SuperHealingPotion;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule normalMode = new LeadingConditionRule(new Conditions.NotExpert());

            /*Materials*/
            normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AtmospheragonScale>(), 1, 4, 10));
            /*Weapons*/
            normalMode.OnSuccess(ItemDropRule.FewFromOptions(2, 1, ModContent.ItemType<WindDragonsClaw>(),
            ModContent.ItemType<StormBow>(), ModContent.ItemType<CelestialSeraphStaff>(),
            ModContent.ItemType<Starfield>(), ModContent.ItemType<SpaceBossWings>()));
            /*Extra*/
            normalMode.OnSuccess(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<AtmospheronMask>(), 7));
            normalMode.OnSuccess(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<FaeTrophyItem>(), 10));

            npcLoot.Add(normalMode);
            /*LootBag*/
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CelestialSeraphBag>()));
        }
        public bool CanAttack;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return CanAttack;
        }
        public override void AI()
        {
            player = Main.player[NPC.target];
            RotationRadians = Vector2.One.RotatedBy(RotationTimer);
            RotationTimer += RotationSpeed;

            SkyManager.Instance.Activate("TranscendenceMod:DragonSky", player.Center);
            CanAttack = true;
            if (Timer_AI < 60 || NPC.velocity == Vector2.Zero)
                CanAttack = false;

            //This thing switches attacks and adjusts some things
            if (Timer_AI > (AttackDuration + 3))
            {
                NPC.velocity = Vector2.Zero;
                NPC.damage = contactDMG;
                NPC.rotation = 0;
                NPC.hide = false;
                moveTwoAngle = 0;
                arenaVisualSize = 0;

                NPC.alpha = 0;

                int INCOMING_ATTACK = Main.rand.Next(1, 2);
                while (INCOMING_ATTACK == NPC.ai[1]) INCOMING_ATTACK = Main.rand.Next(1, 2);
                NPC.ai[1] = INCOMING_ATTACK;

                Opacity = 1f;

                NPC.ai[2] = 0;
                NPC.ai[3] = 0;

                for (int i = 0; i < 5; i++)
                    ProjectileCD[i] = 0;

                Timer_AI = 0;
                RotationTimer = 0;
                RotationSpeed = 0.25f;
                HasArena = false;
            }

            if (local.GetModPlayer<TranscendencePlayer>().RoHTeleports > 3) Enrage();
            else DefaultStats();

            local.AddBuff(ModContent.BuffType<InfiniteFlight>(), 1);

            //Projectile stuff
            ProjectileManagerer();

            NPC.rotation = MathHelper.Lerp(NPC.rotation, NPC.velocity.X * 0.025f, 1f / 20f);

            Timer_AI++;

            //Attack Patterns
            if (NPC.ai[0] == 0)
            {
                switch (NPC.ai[1])
                {
                    case 0: Intro(); break;
                    case 1: Dashes(); break;
                    case 2: Tornadoes(); break;
                }
            }

            //Despawn
            if (player.active == false || player.dead == true)
            {
                if (++DespawnTimer > 60)
                {
                    NPC.active = false;
                    DespawnTimer = 0;
                    SoundEngine.PlaySound(SoundID.Item4);
                    TranscendenceUtils.DustRing(NPC.Center, 25, ModContent.DustType<ArenaDust>(), 24, Color.White, 2);
                }
            }

        }

        public void Intro()
        {
            AttackDuration = 300;

        }

        public void Dashes()
        {
            AttackDuration = 215;
        }

        public void Tornadoes()
        {
            AttackDuration = 212000;

            if (++ProjectileCD[1] % 60 == 0)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(0, 125), Vector2.Zero, Tornado, 90, 2f, -1, 0, NPC.whoAmI);
        }

        private void ProjectileManagerer()
        {
            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile projectile = Main.projectile[p];
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Phase = 0;
            RotationTimer = 0;
            RotationSpeed = 0;
            AttackDuration = 90;
            DefaultStats();

            if (!TranscendenceWorld.EncouteredAtmospheron)
                TranscendenceWorld.EncouteredAtmospheron = true;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ProjectileID.FinalFractal) modifiers.FinalDamage *= 0.25f;
            if (projectile.IsMinionOrSentryRelated) modifiers.FinalDamage *= 0.66f;

            if ((projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed)
                && Main.player[projectile.owner].heldProj == projectile.whoAmI) modifiers.FinalDamage *= 1.33f;
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.DamageType == DamageClass.Melee)
                modifiers.FinalDamage *= 1.33f;
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            //Dust.NewDust(NPC.Center, 1, 1, ModContent.DustType<BetterBlood>(),
                //Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(2f, 4f), 0, Color.White, 1f);
        }

        public override bool PreKill()
        {
            //int arm = Mod.Find<ModGore>("Gore1").Type;
            //int head = Mod.Find<ModGore>("Gore2").Type;
            //int shoulder = Mod.Find<ModGore>("Gore3").Type;
            //int lowerbody = Mod.Find<ModGore>("Gore4").Type;


            if (TranscendenceWorld.DownedWindDragon == false)
            {
                Main.NewText(Language.GetTextValue("Mods.TranscendenceMod.Messages.DragonDeath"), 50, 255, 130);
                TranscendenceWorld.DownedWindDragon = true;
            }

            /*if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < 6; i++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-7, 7)), arm);
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-3, 3)), head);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-5, 5)), shoulder);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.Next(-2, 2)), lowerbody);
            }*/
            return true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.Atmospheron")),
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[NPC.target];
            NPCID.Sets.TrailCacheLength[Type] = 15;
            NPCID.Sets.TrailingMode[Type] = 1;
            if (NPC.velocity.Length() > 2)
                TranscendenceUtils.DrawTrailNPC(NPC, NPC.GetAlpha(drawColor) * Opacity, NPC.scale, $"{Texture}", false, true, 1, NPC.Size / 4, NPC.direction != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            if (HasArena)
            {
                spriteBatch.Draw(TextureAssets.Extra[174].Value,
                    new Rectangle((int)(arenaCenter.X - (ArenaSize * 1.6f * arenaVisualSize) - Main.screenPosition.X),
                    (int)(arenaCenter.Y - (ArenaSize * 1.6f * arenaVisualSize) - Main.screenPosition.Y),
                    (int)(ArenaSize * 2 * 1.6f * arenaVisualSize),
                    (int)(ArenaSize * 2 * 1.6f * arenaVisualSize)),
                    Color.White * 0.33f);
            }
            if (CanAttack)
            {
                for (int i = 0; i < 4; i++)
                {
                    float pi = MathHelper.TwoPi * i / 4;
                    float rot = pi += MathHelper.ToRadians(++NPC.localAI[2] + 4);

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    Vector2 pos = NPC.Center + Vector2.One.RotatedBy(rot) * 10;

                    TranscendenceUtils.DrawEntity(NPC, Color.DeepSkyBlue * 0.65f * Opacity, NPC.scale,
                        $"{Texture}", NPC.rotation, pos, NPC.frame, NPC.direction != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }

            if (CurrentAttack == "Ferocious Predator Strike" && ProjectileCD[1] < 60 && ProjectileCD[1] > 45)
            {
                SpriteBatch sb = Main.spriteBatch;
                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                float rot = dashVel.ToRotation() - MathHelper.PiOver2;
                Vector2 pos = NPC.Center + Vector2.One.RotatedBy(rot + MathHelper.PiOver4) * 720;
                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/ExpandingTelegraph").Value;
                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X),
                    (int)(pos.Y - Main.screenPosition.Y),
                    (ProjectileCD[1] * 2) - 60,
                    2000), null,
                    Color.DeepSkyBlue * 0.5f, rot,
                    sprite.Size() * 0.5f,
                    SpriteEffects.None,
                    0);
                sb.End();
                sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
            float opac = MathHelper.Lerp(0f, 1f, ProjectileCD[1] / 50f) * 0.5f;

            TranscendenceUtils.DrawEntity(NPC, drawColor * Opacity, NPC.scale, Texture, NPC.rotation, NPC.Center, null, NPC.direction != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            if (Phase == 3)
                TranscendenceUtils.DrawTrailNPC(NPC, Color.White * opac, NPC.scale, $"{Texture}_Glow", false, true, 1, NPC.Size / 4, NPC.direction != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            return false;
        }
    }
}