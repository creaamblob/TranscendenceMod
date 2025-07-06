using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Buffs.Items.InfectionAccessories;
using TranscendenceMod.Buffs.Items.Weapons;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items;
using TranscendenceMod.Items.Accessories;
using TranscendenceMod.Items.Accessories.Other;
using TranscendenceMod.Items.Accessories.Shields;
using TranscendenceMod.Items.Accessories.Vanity;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Items.Consumables.FoodAndDrinks;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.Items.Farming.Seeds;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Tools;
using TranscendenceMod.Items.Tools.Compasses;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Items.Weapons.Ranged;
using TranscendenceMod.Miscanellous.UI.Achievements.Tasks;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Miscannellous.UI.ModifierUI;
using TranscendenceMod.NPCs;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.NPCs.PreHard;
using TranscendenceMod.NPCs.SpaceBiome;
using TranscendenceMod.NPCs.SpaceBiome.Worm;
using TranscendenceMod.Projectiles.Equipment;
using TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;
using TranscendenceMod.Tiles.TilesheetHell.Nature;
using Conditions = Terraria.GameContent.ItemDropRules.Conditions;

namespace TranscendenceMod.Miscannellous.GlobalStuff
{
    public class BossNoHit : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => false;
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => Language.GetTextValue("Mods.TranscendenceMod.Messages.BossNoHit");
    }
    public class DragonDropRule : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info) => TranscendenceWorld.DownedWindDragon;
        public bool CanShowItemDropInUI() => TranscendenceWorld.DownedWindDragon;
        public string GetConditionDescription() => Language.GetTextValue("Mods.TranscendenceMod.Messages.DragonCondition");
    }
    public class TranscendenceNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool DeepseaDebuff;
        public bool EarthernDebuff;
        public bool Incinerated;
        public bool Unmovable;
        public int SuckedInNecklace;

        public Vector2 dashPos;
        public bool PossessionAvaivable;
        public bool Possessed;
        public int PossessedTimer;
        public Vector2 Possesspos;
        public Player TargetPlayer;
        public int NotPossessTimer;
        public int PossessionJumpCD;
        public int Stunned;

        public int SlowDownType;
        public float NPCSlownessMultiplier = 1;

        public int HitCD;

        public bool Nohit;
        public int TimeSpent;

        public int Worm;
        public bool Gasolined;
        public NPC parent;

        public bool EmpressChallenge;

        public override bool? CanChat(NPC npc)
        {
            if (ModifierApplierUIDrawing.Visible)
                return false;
            return base.CanChat(npc);
        }
        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == NPCID.Demolitionist)
            {
                if (Main.hardMode && Main.rand.NextBool(6))
                    chat = Language.GetTextValue("Mods.TranscendenceMod.Messages.DemolitionistSuperBombHint");
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            Player p = Main.player[projectile.owner];
            if (p.GetModPlayer<TranscendencePlayer>().Eternity)
                modifiers.DisableCrit();

            if (npc.HasBuff(ModContent.BuffType<StarfieldDebuff>()) && projectile.IsMinionOrSentryRelated)
            {
                modifiers.FinalDamage *= 1.15f;
            }
            if (npc.HasBuff(ModContent.BuffType<OvergrownDebuff>()) && projectile.IsMinionOrSentryRelated)
            {
                modifiers.FinalDamage *= 1.1f;
            }
        }
        public override void ModifyHitPlayer(NPC npc, Player p, ref Player.HurtModifiers modifiers)
        {
        }
        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            return base.CanBeHitByProjectile(npc, projectile);
        }
        public static bool zoneUnderground = Main.LocalPlayer.ZoneDirtLayerHeight;
        public static bool zoneCaverns = Main.LocalPlayer.ZoneRockLayerHeight;
        public static bool zoneGrav = Main.LocalPlayer.ZoneGraveyard;
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
        }
        public bool OnFire(NPC npc)
        {
            return (npc.onFire || npc.onFire2 || npc.onFire3 || npc.onFrostBurn || npc.onFrostBurn2 || npc.HasBuff(ModContent.BuffType<PrismaticBurn>()));
        }

        public bool PostPlantDungSkeleton(NPC npc)
        {
            return (npc.type == NPCID.SkeletonSniper || npc.type == NPCID.BoneLee || npc.type == NPCID.SkeletonCommando || npc.type == NPCID.TacticalSkeleton
                || npc.type == NPCID.Necromancer || npc.type == NPCID.NecromancerArmored || npc.type == NPCID.Paladin);
        }
        public bool PostPlantDungGenericSkeleton(NPC npc)
        {
            return (npc.type == NPCID.BlueArmoredBones || npc.type == NPCID.BlueArmoredBonesMace || npc.type == NPCID.BlueArmoredBonesNoPants || npc.type == NPCID.BlueArmoredBonesSword ||
                npc.type == NPCID.HellArmoredBones || npc.type == NPCID.HellArmoredBonesMace || npc.type == NPCID.HellArmoredBonesSpikeShield || npc.type == NPCID.HellArmoredBonesSword ||
                npc.type == NPCID.RustyArmoredBonesAxe || npc.type == NPCID.RustyArmoredBonesFlail || npc.type == NPCID.RustyArmoredBonesSword || npc.type == NPCID.RustyArmoredBonesSwordNoArmor ||
                npc.type == NPCID.GiantCursedSkull || npc.type == NPCID.DungeonSpirit);
        }
        public bool DungSkeleton(NPC npc)
        {
            return (npc.type == NPCID.AngryBones || npc.type == NPCID.AngryBonesBig || npc.type == NPCID.AngryBonesBigHelmet || npc.type == NPCID.AngryBonesBigMuscle ||
            npc.type == NPCID.ShortBones || npc.type == NPCID.BigBoned || npc.type == NPCID.DarkCaster || npc.type == NPCID.CursedSkull);
        }

        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            LeadingConditionRule normalMode = new LeadingConditionRule(new Conditions.NotExpert());

            if (npc.type == NPCID.HallowBoss)
            {
                normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EasternTalismans>(), 3));
                normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ChromaticAegis>(), 4));
            }

            if (npc.type == NPCID.BloodZombie || npc.type == NPCID.Drippler)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TomatoSeeds>(), 20));

            if (npc.type == NPCID.SnowmanGangsta)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GangstaShotgun>(), 50));

            if (npc.type == NPCID.Golem)
                normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HealthyJewel>(), 3));

            if (npc.type == NPCID.DD2Betsy)
                normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DragonScale>(), 1, 2, 3));

            if (npc.type == NPCID.HallowBoss)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartOfTheQueen>()));

            if (npc.type == NPCID.MoonLordCore)
            {
                normalMode.OnSuccess(ItemDropRule.Common(ItemID.LongRainbowTrailWings, 3));
                normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SlayerOfGiants>(), 1, 2, 3));
                normalMode.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LunarShield>(), 2));
            }

            if (npc.type == NPCID.WallofFlesh) npcLoot.Add(ItemDropRule.ByCondition(new BossNoHit(), ModContent.ItemType<ForgottenInferno>()));

            if (npc.type == NPCID.Mothron) npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MothronLamp>(), 3));

            if (npc.type == NPCID.Pumpking) npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkullMasterSickle>(), 8));

            if (npc.type == NPCID.Deerclops) npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ConstantDial>()));

            if (DungSkeleton(npc))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LegCutter>(), 66));
                npcLoot.Add(ItemDropRule.ByCondition(new DragonDropRule(), ModContent.ItemType<PoseidonsTide>(), 6));
            }

            if (PostPlantDungGenericSkeleton(npc) && (npc.type == NPCID.BlueArmoredBones || npc.type == NPCID.BlueArmoredBonesMace || npc.type == NPCID.BlueArmoredBonesNoPants || npc.type == NPCID.BlueArmoredBonesSword))
                npcLoot.Add(ItemDropRule.ByCondition(new DragonDropRule(), ModContent.ItemType<PoseidonsTide>(), 3));

            if (PostPlantDungSkeleton(npc))
                npcLoot.Add(ItemDropRule.ByCondition(new DragonDropRule(), ModContent.ItemType<PoseidonsTide>(), 1, 1, 3));
        }
        public override bool CheckDead(NPC npc)
        {
            if (npc.target != -1 && !Main.player[npc.target].GetModPlayer<TranscendencePlayer>().Possessing && Main.player[npc.target].GetModPlayer<TranscendencePlayer>().CorruptWanderingKit && !PossessionAvaivable
                && Main.player[npc.target].Distance(npc.Center) < 1000 && !npc.boss && npc.realLife == -1 && !Possessed
                && npc.type != NPCID.PirateShip && npc.type != NPCID.PirateShipCannon && npc.type != NPCID.GolemHead && npc.type != NPCID.EaterofWorldsHead && !(npc.ModNPC is HeadSegment))
            {
                TargetPlayer = Main.player[npc.target];

                PossessionAvaivable = true;
                npc.dontTakeDamage = true;
                npc.life = npc.lifeMax;
                npc.velocity = Vector2.Zero;
                return false;
            }

            return base.CheckDead(npc);
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            int scrolls = player.GetModPlayer<TranscendencePlayer>().CultScrollsEquipped;
            if (scrolls > 0)
            {
                if (npc.boss)
                    modifiers.NonCritDamage -= (0.1f * scrolls);
                else modifiers.FinalDamage += (0.25f * scrolls);
            }
            
            if (player.GetModPlayer<TranscendencePlayer>().Eternity)
                modifiers.DisableCrit();
        }
        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if (Possessed && player == TargetPlayer) return false;
            return base.CanBeHitByItem(npc, player, item);
        }

        public override void ResetEffects(NPC npc)
        {
            if (Stunned > 0) Stunned--;
            if (HitCD > 0) HitCD--;
            if (SuckedInNecklace > 0) SuckedInNecklace--;
            Gasolined = false;
            SlowDownType = 0;
            NPCSlownessMultiplier = 1;
            DeepseaDebuff = false;
            Incinerated = npc.HasBuff(ModContent.BuffType<Incineration>());
        }
        public override void OnKill(NPC npc)
        {
            if (npc.boss && TimeSpent > 15)
            {
                DialogUI.SpawnDialog("Time Spent: " + (TimeSpent / 60).ToString() + "s", npc.Center, 450, Color.Red);
            }

            if (npc.type == NPCID.DD2Betsy && !TranscendenceWorld.DownedOOA)
                TranscendenceWorld.DownedOOA = true;

            if (npc.type == NPCID.EyeofCthulhu & !NPC.downedBoss1)
            {
                Main.NewText(Language.GetTextValue("Mods.TranscendenceMod.Messages.HardmetalOreUnlock"), 50, 255, 130);
                for (int i = 200; i < (Main.maxTilesX - 200); i++)
                {
                    for (int j = 0 + (int)(Main.maxTilesY / 2f); j < (Main.maxTilesY - 400); j++)
                    {
                        Tile tile = Main.tile[i, j];
                        if ((tile.TileType == TileID.Stone && Main.rand.NextBool(1825)) && tile.HasTile)
                        {
                            WorldGen.OreRunner(i, j, 6, 42, (ushort)ModContent.TileType<HardmetalOreTile>());
                        }
                    }
                }
            }

            if (npc.type == NPCID.WallofFlesh && Nohit)
                Item.NewItem(npc.GetSource_Death(), npc.getRect(), ModContent.ItemType<ForgottenInferno>());

            if (!npc.boss && !npc.friendly && npc.lifeMax > 5 && Main.rand.NextBool(50) && Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().HasSurvivorKnife > 0)
            {
                int item = ModContent.ItemType<Meat>();
                int amount = 1;

                switch (Main.rand.Next(0, 4))
                {
                    case 0: item = ModContent.ItemType<Meat>(); break;
                    case 1: item = ModContent.ItemType<ScavengerChisel>(); break;
                    case 2: item = ModContent.ItemType<SturdyPlate>(); break;
                    case 3: item = ItemID.Dynamite; amount = 3; break;
                    case 4: Item.NewItem(npc.GetSource_Death(), npc.getRect(), ModContent.ItemType<VineGun>()); item = ItemID.VineRopeCoil; amount = 10; break;
                }
                Item.NewItem(npc.GetSource_Death(), npc.getRect(), item, amount);
            }

            if (npc.type == NPCID.MoonLordCore && !NPC.downedMoonlord)
                Main.NewText(Language.GetTextValue("Mods.TranscendenceMod.Messages.MoonlordDeath"), 50, 255, 130);
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.type == NPCID.HallowBoss)
                EmpressChallenge = true;

            if (source is EntitySource_Parent npc2 && npc2.Entity is NPC npc3 && npc3 != null && npc3.active)
                parent = npc3;

            Nohit = true;
            if (npc.boss)
                TimeSpent = 0;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.HasBuff<JungleRingBuff>())
                drawColor = Color.DarkOliveGreen;

            if (npc.type == NPCID.SpikeBall)
                drawColor = Color.Lerp(drawColor, Color.DarkBlue, 0.5f);

            if (npc.HasBuff<BloodLoss>())
                drawColor = Color.DarkGray;

            if (npc.HasBuff<PrismaticBurn>())
                drawColor = Main.dayTime ? Color.Goldenrod : Main.DiscoColor;

            if (Gasolined)
                drawColor = new Color(0.4f, 0.4f, 0.4f);
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(ModContent.BuffType<FrostBite>()) && modifiers.DamageType == DamageClass.Summon)
                modifiers.Defense -= 20;
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (SuckedInNecklace > 0)
                return false;

            // Parrying
            if (npc.active && base.CanHitPlayer(npc, target, ref cooldownSlot) && npc.Hitbox.Intersects(target.Hitbox) && !npc.dontTakeDamage && npc != null && TranscendenceUtils.GeneralParryConditions(target))
            {
                target.TryGetModPlayer(out TranscendencePlayer modPlayer);
                if (modPlayer == null)
                    return base.CanHitPlayer(npc, target, ref cooldownSlot);

                DialogUI.SpawnDialog(Language.GetTextValue("Mods.TranscendenceMod.Messages.Parry"), target.Top, 60, Color.Gold);

                Projectile.NewProjectile(target.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<ParryVisual>(), 0, 0, target.whoAmI, target.GetModPlayer<TranscendencePlayer>().ShieldID);

                if (modPlayer.PalladiumShieldEquipped)
                    target.AddBuff(BuffID.RapidHealing, 240);

                target.SetImmuneTimeForAllTypes(45);
                SoundEngine.PlaySound(SoundID.DrumCymbal2, target.Center);

                int dmg = npc.damage < 250 ? npc.damage * 3 : 750;
                npc.SimpleStrikeNPC(dmg, target.direction, false, 4f, DamageClass.Generic);

                modPlayer.ParryTimer = 0;
                modPlayer.ParryTimerCD = 0;
                modPlayer.Focus -= modPlayer.ParryFocusCost;

                return false;
            }

            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override void AI(NPC npc)
        {
            if (npc != null && npc.target != -1 && npc.target < Main.maxPlayers && npc.active && Main.player[npc.target] != null && Main.player[npc.target].active)
            {
                Player player = Main.player[npc.target];

                if (player != null && player.active && !player.dead && player.TryGetModPlayer(out TranscendencePlayer modplayer) && modplayer != null && modplayer.HitTimer > 0)
                {
                    Nohit = false;
                }

                if ((npc.lavaWet || Collision.SolidCollision(npc.position, npc.width, npc.height)) && player.TryGetModPlayer(out TranscendencePlayer modplayer2) && modplayer2 != null && modplayer2.DangerDetection)
                {
                    Dust.NewDust(npc.Top, npc.width, npc.height, DustID.TreasureSparkle);
                }
            }

            if (OnFire(npc) && Gasolined)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC nPC = Main.npc[i];
                    if (nPC != null && nPC.active && nPC.Distance(npc.Center) < 250 && nPC != npc && !OnFire(nPC) && nPC.GetGlobalNPC<TranscendenceNPC>().Gasolined)
                    {
                        nPC.AddBuff(BuffID.OnFire3, 180);
                    }
                }
            }

            if (Incinerated && Gasolined)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC nPC = Main.npc[i];
                    if (nPC != null && nPC.active && nPC.Distance(npc.Center) < 250 && nPC != npc && !nPC.GetGlobalNPC<TranscendenceNPC>().Incinerated)
                    {
                        nPC.AddBuff(ModContent.BuffType<Incineration>(), 30);
                    }
                }
            }

            if (npc.boss)
                TimeSpent++;

            if (Incinerated)
            {
                Vector2 pos = new Vector2(Main.rand.Next(-npc.width, npc.width), npc.height);
                Dust.NewDustPerfect(npc.Center - pos, ModContent.DustType<TrailDustHQ>(), new Vector2(0, 5), 0, Color.Red, 1.85f);
                Dust.NewDustPerfect(npc.Center - pos, ModContent.DustType<TrailDustHQ>(), new Vector2(0, 5), 0, Color.Orange);
            }

            if (npc.HasBuff<PrismaticBurn>())
            {
                if (Main.rand.NextBool(2))
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<ArenaDust>(), 0, 0, 0, Main.DiscoColor, 0.75f);
                }
            }


            if (Gasolined)
            {
                if (Main.rand.NextBool(4))
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Oil>(), 0, 0, 0, Color.White, 1f);
                    Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Oil>(), 0, 0, 0, Color.White, 1f);
                }
            }
        }

        public override bool PreKill(NPC npc)
        {
            if (npc.type == NPCID.HallowBoss && EmpressChallenge && npc.target != -1)
                ModAchievementsHelper.CompleteChallenge(Main.player[npc.target], TaskIDs.EmpressChallenge);

            return base.PreKill(npc);
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (NPC.AnyNPCs(NPCID.EmpressButterfly))
                pool.Remove(NPCID.EmpressButterfly);

            if (spawnInfo.Player.InModBiome<Heaven>() || spawnInfo.Player.InModBiome<Limbo>())
                pool.Remove(0);

            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneLandSite || spawnInfo.Player.ZoneShimmer ||
                TranscendenceUtils.BossAlive() && !NPC.AnyNPCs(NPCID.LunarTowerSolar) && !NPC.AnyNPCs(NPCID.LunarTowerVortex)
                 && !NPC.AnyNPCs(NPCID.LunarTowerNebula) && !NPC.AnyNPCs(NPCID.LunarTowerStardust) 
                 || NPC.AnyNPCs(NPCID.EaterofWorldsHead) || spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneSpaceTemple)
                pool.Clear();
        }

        public override bool? CanFallThroughPlatforms(NPC npc)
        {
            if (Possessed)
            {
                if (TargetPlayer != null && TargetPlayer.active && TargetPlayer.controlDown) return true;
                else return false;
            }
            return base.CanFallThroughPlatforms(npc);
        }


        public override bool PreAI(NPC npc)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()) && npc.aiStyle == 7)
            {
                npc.Bottom = new Vector2(npc.homeTileX * 16, npc.homeTileY * 16);
                npc.velocity = Vector2.Zero;
                return false;
            }

            if (npc != null && npc.target != -1 && npc.target < Main.maxPlayers && npc.active && Main.player[npc.target] != null && Main.player[npc.target].active)
            {
                Player player = Main.player[npc.target];
                Player lPlayer = Main.LocalPlayer;

                if (player != null && npc.type == NPCID.HallowBoss)
                {
                    if (npc.ai[0] > 0 && !player.GetModPlayer<TranscendencePlayer>().LacewingTrans)
                        EmpressChallenge = false;
                    else
                    {
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile p = Main.projectile[i];
                            if (p != null && p.active && p.owner == player.whoAmI && !p.hostile && p.type != ModContent.ProjectileType<EmpressSun>() && p.damage > 0 || p.type == ProjectileID.FallingStarSpawner || p.type == ProjectileID.FallingStar)
                                p.active = false;
                        }
                    }
                }

                if (DungSkeleton(npc) && npc.aiStyle == NPCAIStyleID.Fighter)
                {
                    if (npc.wet && npc.velocity.Y > 0)
                            npc.velocity.Y *= 0.99f;
                }
            }

            if (Stunned > 0)
            {
                npc.velocity *= 0.9f;
                return false;
            }

            if (PossessionAvaivable && npc.active && TargetPlayer != null && npc.life > 0 && !TargetPlayer.dead)
            {
                if (TargetPlayer.Distance(npc.Center) < 250 && TargetPlayer.GetModPlayer<TranscendencePlayer>().InfectionAbility && !TargetPlayer.GetModPlayer<TranscendencePlayer>().Possessing)
                {
                    Dust.QuickDustLine(TargetPlayer.Center, npc.Center, 20, Color.Purple);
                    Dust.QuickDustLine(TargetPlayer.Center, npc.Center, 30, Color.Green);

                    Possessed = true;
                }
                if (Possessed)
                {
                    if (npc.Distance(TranscendenceWorld.WorldCenter) < (1500 * 1.35f) || TargetPlayer.dead)
                        npc.StrikeInstantKill();

                    npc.dontTakeDamage = true;
                    npc.friendly = true;

                    NPCID.Sets.ImmuneToAllBuffs[npc.type] = true;

                    TargetPlayer.GetModPlayer<TranscendencePlayer>().cameraPos = npc.Center;
                    TargetPlayer.GetModPlayer<TranscendencePlayer>().cameraModifier = true;

                    TargetPlayer.GetModPlayer<TranscendencePlayer>().Possessing = true;
                    TargetPlayer.GetModPlayer<TranscendencePlayer>().PossessingTimer = 0;
                    TargetPlayer.GetModPlayer<TranscendencePlayer>().PossessedNPC = npc;
                    TargetPlayer.direction = npc.direction;

                    //Set player defense to npc defense
                    TargetPlayer.statDefense -= TargetPlayer.statDefense;
                    TargetPlayer.statDefense += npc.defense;

                    npc.velocity *= 0.95f;

                    if (npc.type == NPCID.MourningWood || npc.type == NPCID.Everscream || npc.type == NPCID.SantaNK1 || npc.type == NPCID.IceGolem)
                    {
                        npc.noGravity = false;
                        npc.noTileCollide = false;
                    }

                    PossessedTimer++;

                    if (npc.noGravity || npc.type == NPCID.SandElemental)
                    {
                        if (npc.aiStyle == NPCAIStyleID.Spider || npc.aiStyle == NPCAIStyleID.Flying || npc.aiStyle == NPCAIStyleID.DemonEye || npc.aiStyle == NPCAIStyleID.EnchantedSword || npc.type == NPCID.ServantofCthulhu ||
                            npc.type == ModContent.NPCType<Scorpio>() || npc.type == ModContent.NPCType<SpaceJelly>() || npc.type == ModContent.NPCType<StormEel>())
                        {
                            float extraRot = 0;
                            if (npc.type == NPCID.ServantofCthulhu || npc.aiStyle == NPCAIStyleID.Flying) extraRot = MathHelper.Pi;
                            if (npc.aiStyle == NPCAIStyleID.EnchantedSword) extraRot = MathHelper.PiOver4 * npc.direction ;
                            if (npc.type == ModContent.NPCType<StormEel>()) extraRot = MathHelper.PiOver2;
                            if (npc.aiStyle == NPCAIStyleID.Spider) extraRot = MathHelper.PiOver2 * npc.direction;
                            npc.rotation = npc.DirectionTo(Main.MouseWorld).ToRotation()
                                + MathHelper.PiOver2 + extraRot;
                        }
                        else npc.rotation = 0;
                        if (npc.Center.X > Main.MouseWorld.X)
                        {
                            npc.direction = -1;
                            npc.spriteDirection = -1;
                        }
                        else
                        {
                            npc.direction = 1;
                            npc.spriteDirection = 1;
                        }
                        if (TargetPlayer.controlJump && npc.Distance(Main.MouseWorld) > 50)
                            npc.velocity = npc.DirectionTo(Main.MouseWorld) * 10;
                    }
                    else
                    {
                        npc.rotation = 0;
                        npc.spriteDirection = npc.direction;
                        float speed = npc.aiStyle == NPCAIStyleID.Unicorn ? 20 : 10;

                        if (TargetPlayer.controlLeft)
                        {
                            npc.direction = -1;
                            npc.velocity.X -= speed / 20f;
                        }
                        if (TargetPlayer.controlRight)
                        {
                            npc.direction = 1;
                            npc.velocity.X += speed / 20f;
                        }
                        if (Collision.SolidCollision(npc.BottomLeft, npc.width, 1, true) && TargetPlayer.controlJump)
                        {
                            npc.velocity.Y = -25;
                        }
                    }

                    if (TranscendenceWorld.InfectionAccessoryKeyBind.JustPressed && PossessedTimer > 5 && !Collision.SolidCollision(TargetPlayer.position, TargetPlayer.width, TargetPlayer.height) || TargetPlayer.dead)
                    {
                        TargetPlayer.GetModPlayer<TranscendencePlayer>().PossessingTimer = 10;
                        TargetPlayer.velocity = TargetPlayer.DirectionTo(Main.MouseWorld) * 25;
                        for (int i = 0; i < 65; i++)
                        {
                            Dust d = Dust.NewDustPerfect(npc.Center, DustID.Blood, Main.rand.NextVector2Circular(15, 15), 0, Color.White, 3);
                            d.noGravity = true;
                        }
                        npc.StrikeInstantKill();
                    }
                }
                else
                {
                    Dust.NewDustPerfect(npc.Center, ModContent.DustType<ArenaDust>(), new Vector2(0, -10), 0, Color.GreenYellow, 1f);
                    npc.velocity = Vector2.Zero;
                    if (++NotPossessTimer > 300)
                        npc.StrikeInstantKill();
                }
                return false;
            }
 
            return base.PreAI(npc);
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (Incinerated)
                npc.lifeRegen -= 15;
            if (npc.lifeRegen < 0 && Gasolined)
            {
                npc.lifeRegen *= 3;
            }
        }
        public override void PostAI(NPC npc)
        {
            if (npc.type == NPCID.TargetDummy)
            {
                if (TranscendenceUtils.BossAlive())
                    npc.dontTakeDamage = true;
                else npc.dontTakeDamage = false;
            }

            if (SlowDownType == 1)
                npc.velocity *= 0.95f;

            if (SlowDownType == 2)
                npc.velocity *= 0.9f;

            if (SlowDownType == 3)
                npc.velocity *= 0.75f;

            if (npc.HasBuff(ModContent.BuffType<SpaceDebuff>()))
            {
                Dust.NewDustPerfect(npc.Bottom + new Vector2((float)Math.Sin(Main.GlobalTimeWrappedHourly) * 25f, -5), ModContent.DustType<ExtraTerrestrialDust>(), new Vector2(0, -5), 0, default, 0.5f);
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (EarthernDebuff)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<TranscendencePlayer>().ZoneStar)
                spawnRate = (int)(spawnRate * 0.75f);

            if (player.GetModPlayer<TranscendencePlayer>().ZoneVolcano)
            {
                spawnRate = (int)(spawnRate * 0.5f);
                maxSpawns = (int)(maxSpawns * 1.33f);
            }
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()) && npc.aiStyle == 7)
                return false;

            if (EarthernDebuff)
            {
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/NoiseShader", AssetRequestMode.ImmediateLoad).Value;
                //Apply Earthern Potato Field Texture
                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/EarthMagicShader").Value;
                Main.instance.GraphicsDevice.Textures[1] = sprite;

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                eff.Parameters["uImageSize0"].SetValue(TextureAssets.Npc[npc.type].Size() + new Vector2(0, npc.frame.Y * 3));
                eff.Parameters["uImageSize1"].SetValue(sprite.Size());
                eff.CurrentTechnique.Passes["NoiseTechnique2"].Apply();

            }

            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.TargetDummy)
                Unmovable = true;

            if (!NPC.downedBoss1 && npc.type == NPCID.Ghost)
            {
                npc.lifeMax = 30;
                npc.knockBackResist = 1.25f;
                npc.scale = 0.75f;
            }

            if (npc.type == NPCID.WaterSphere && TranscendenceWorld.DownedWindDragon)
            {
                npc.dontTakeDamage = true;
                npc.lifeMax = 1750;
                npc.damage = 200;
            }

            if (DungSkeleton(npc) && TranscendenceWorld.DownedWindDragon)
            {
                npc.lifeMax *= 16;
                npc.defense *= 2;
                npc.damage *= 4;
                npc.knockBackResist = 0f;
            }

            if (PostPlantDungGenericSkeleton(npc) && TranscendenceWorld.DownedWindDragon)
            {
                npc.lifeMax *= 4;
                npc.damage *= 3;
                npc.knockBackResist = 0f;
            }

            if (PostPlantDungSkeleton(npc) && TranscendenceWorld.DownedWindDragon)
            {
                npc.lifeMax *= npc.type == NPCID.Paladin ? 4 : npc.type == NPCID.BoneLee ? 8 : 16;
                npc.damage *= 2;
                npc.knockBackResist = 0f;
            }

            if (npc.type == NPCID.TheHungry || npc.type == NPCID.TheHungryII)
            {
                npc.lifeMax = 55;
                npc.knockBackResist = 1.5f;
            }

            if (npc.type == NPCID.SkeletronHand || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight)
                npc.buffImmune[ModContent.BuffType<EarthernFortification>()] = true;

            if (npc.type == NPCID.HallowBoss)
                npc.buffImmune[ModContent.BuffType<PrismaticBurn>()] = true;

            if (npc.buffImmune[BuffID.Venom] == true) npc.buffImmune[ModContent.BuffType<JungleRingBuff>()] = true;
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(ModContent.ItemType<SurvivorKnife>());
                shop.Add(ModContent.ItemType<Compass>());
            }

            if (shop.NpcType == NPCID.Mechanic)
                TranscendenceUtils.sell(shop, ModContent.ItemType<NohitMode>());

            if (shop.NpcType == NPCID.GoblinTinkerer)
            {
                TranscendenceUtils.sell(shop, ItemID.Toolbox, Item.buyPrice(gold: 10), Condition.DownedDeerclops);
                TranscendenceUtils.sell(shop, ItemID.AncientChisel, Item.buyPrice(gold: 5, silver: 35), Condition.InDesert);
                TranscendenceUtils.sell(shop, ItemID.TallyCounter, Item.buyPrice(gold: 14, silver: 99), Condition.Hardmode, Condition.DownedSkeletron);
                TranscendenceUtils.sell(shop, ItemID.Binoculars, Item.buyPrice(gold: 24, silver: 95), Condition.DownedEyeOfCthulhu);
            }
        }
    }
}


