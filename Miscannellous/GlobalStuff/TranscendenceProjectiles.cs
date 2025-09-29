using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables;
using TranscendenceMod.Projectiles.Equipment;
using TranscendenceMod.Projectiles.Modifiers;
using TranscendenceMod.Projectiles.Weapons.Melee;
using TranscendenceMod.Projectiles.Weapons.Ranged;
using TranscendenceMod.Projectiles.Weapons.Summoner;
using static TranscendenceMod.TranscendenceWorld;

namespace TranscendenceMod.Miscannellous.GlobalStuff
{
    public class TranscendenceProjectiles : GlobalProjectile
    {
        public int StellarDirection = 0;
        public int SpaceBossPortalProjectile = 1;
        public bool Grazed;
        public bool CustomCollision;
        public int Timer;
        public int Timer2;
        public bool CanBeTimeStopped;
        public int PreTimeStopTimeLeft;
        public Vector2 PreTimeStopVel;
        public Vector2 baseVel;
        public Vector2 startPos;
        public bool IsABullet;
        public bool CanBeErased = true;
        public int StupidInt;
        public Vector2 GoToPos;
        public bool WentToPos = false;
        public bool WentToPos2 = false;
        public bool SnowArrow;
        public float vel;
        public NPC owner;
        public override bool InstancePerEntity => true;

        public bool FromSFtB;
        public bool FromMinigun;

        public override void SetDefaults(Projectile projectile)
        {
            base.SetDefaults(projectile);
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitPlayer(projectile, target, ref modifiers);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];

            if (SnowArrow)
                target.AddBuff(BuffID.Frostburn2, 180);
        }
        public bool ModUnparryable;
        public bool Unparryable(Projectile projectile)
        {
            return projectile.type == ProjectileID.EyeFire || projectile.type == ProjectileID.DD2BetsyFlameBreath
                || projectile.type == ProjectileID.FairyQueenSunDance || projectile.type == ProjectileID.HallowBossLastingRainbow || ModUnparryable;
        }
        public bool parryConditions(Projectile projectile, Player target)
        {
            return projectile.hostile && projectile.damage > 0 && !Unparryable(projectile) && TranscendenceUtils.GeneralParryConditions(target);
        }
        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            if (projectile.type == ProjectileID.PoisonSeedPlantera && projectile.ai[0] < 1)
                return projectile.localAI[1] > 45;

            if (projectile.type == ProjectileID.PhantasmalDeathray || projectile.type == ProjectileID.PhantasmalSphere)
            {
                if (target.GetModPlayer<TranscendencePlayer>().FairerMoonlord)
                    return false;

                return base.CanHitPlayer(projectile, target);
            }

            if (projectile.type == ProjectileID.HallowBossRainbowStreak && projectile.timeLeft < 60)
                return false;

            if (parryConditions(projectile, target) && base.CanHitPlayer(projectile, target))
            {
                DoParry(target, projectile);
                return false;
            }
            if (target.GetModPlayer<TranscendencePlayer>().ShieldIFrames > 0)
                return false;
            return base.CanHitPlayer(projectile, target);
        }

        public void DoParry(Player target, Projectile projectile)
        {
            target.TryGetModPlayer(out TranscendencePlayer modplayer);
            if (modplayer == null)
                return;

            DialogUI.SpawnDialogCutscene(Language.GetTextValue("Mods.TranscendenceMod.Messages.Parry"), DialogBoxes.Generic, 1, 1, target, new Vector2(0, -target.height - 40), 90, Color.Gold);

            Projectile.NewProjectile(target.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<ParryVisual>(), 0, 0, target.whoAmI, target.GetModPlayer<TranscendencePlayer>().ShieldID);

            if (modplayer.EolAegis)
            {
                TranscendenceUtils.ProjectileRing(target, 9, target.GetSource_FromAI(), target.Center, ModContent.ProjectileType<EolShieldLaser>(), 160, 0, 2, 0, 0, 1, target.whoAmI, 0);
                TranscendenceUtils.ProjectileRing(target, 9, target.GetSource_FromAI(), target.Center, ModContent.ProjectileType<EolShieldLaser>(), 160, 0, 2, 0, 0, -1, target.whoAmI, 0);
                SoundEngine.PlaySound(SoundID.Item33 with { MaxInstances = 0 }, target.Center);
            }

            target.velocity = Vector2.Zero;
            SoundEngine.PlaySound(SoundID.DrumCymbal2, target.Center);

            modplayer.ParryTimer = 0;
            modplayer.ParryTimerCD = 0;
            modplayer.ShieldIFrames = 60;
            modplayer.Focus -= 35f;
            
            target.SetImmuneTimeForAllTypes(45);
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = Main.player[projectile.owner];

            startPos = projectile.Center;
            baseVel = projectile.velocity;

            if (source is EntitySource_Parent npc && npc.Entity is NPC npc2 && npc2 != null && npc2.active)
                owner = npc2;

            if ((projectile.type == ProjectileID.PaladinsHammerHostile && owner.type == NPCID.Paladin ||
                projectile.type == ProjectileID.LostSoulHostile && (owner.type == NPCID.RaggedCaster || owner.type == NPCID.RaggedCasterOpenCoat)) && Downed.Contains(Bosses.Atmospheron) && owner != null)
            {
                projectile.damage *= 2;
                projectile.velocity *= 1.75f;
            }

            if (projectile.type == ProjectileID.ShadowBeamHostile && Downed.Contains(Bosses.Atmospheron) && owner != null && (owner.type == NPCID.Necromancer || owner.type == NPCID.NecromancerArmored))
            {
                projectile.extraUpdates = 3;
                projectile.timeLeft *= 6;
                projectile.damage *= 2;
            }

            base.OnSpawn(projectile, source);
        }
        public override bool PreAI(Projectile projectile)
        {
            Player owner2 = Main.player[projectile.owner];

            int sx = TranscendenceWorld.SpaceTempleX;
            if ((projectile.type == ProjectileID.Bomb || projectile.type == ProjectileID.Dynamite || projectile.type == ProjectileID.StickyBomb ||
                projectile.type == ProjectileID.StickyDynamite || projectile.type == ProjectileID.BouncyBomb ||
                projectile.type == ProjectileID.BouncyDynamite || projectile.type == ProjectileID.ScarabBomb ||
                projectile.type == ProjectileID.RocketIV || projectile.type == ProjectileID.RocketII || projectile.type == ProjectileID.MiniNukeGrenadeII
                || projectile.type == ProjectileID.MiniNukeMineII || projectile.type == ProjectileID.MiniNukeRocketII || projectile.type == ProjectileID.MiniNukeSnowmanRocketII
                || projectile.type == ProjectileID.RocketSnowmanII || projectile.type == ProjectileID.RocketSnowmanIV
                || projectile.type == ProjectileID.MiniNukeMineII || projectile.type == ProjectileID.ClusterFragmentsI
                || projectile.type == ProjectileID.GrenadeII || projectile.type == ProjectileID.ProximityMineIV
                || projectile.type == ProjectileID.RocketSnowmanII || projectile.type == ProjectileID.RocketSnowmanIV
                || projectile.type == ProjectileID.ProximityMineIV || projectile.type == ProjectileID.Celeb2RocketExplosive
                || projectile.type == ProjectileID.Celeb2RocketExplosiveLarge || projectile.type == ProjectileID.GrenadeIV
                || projectile.type == ProjectileID.ClusterMineII || projectile.type == ProjectileID.ClusterRocketII
                || projectile.type == ProjectileID.LavaBomb || projectile.type == ProjectileID.LavaGrenade
                || projectile.type == ProjectileID.LavaMine || projectile.type == ProjectileID.LavaSnowmanRocket
                || projectile.type == ProjectileID.LavaRocket || projectile.type == ProjectileID.WetBomb || projectile.type == ProjectileID.WetGrenade
                || projectile.type == ProjectileID.WetRocket || projectile.type == ProjectileID.WetMine || projectile.type == ProjectileID.WetSnowmanRocket
                || projectile.type == ProjectileID.HoneyBomb || projectile.type == ProjectileID.HoneyGrenade || projectile.type == ProjectileID.HoneyMine
                || projectile.type == ProjectileID.HoneyRocket || projectile.type == ProjectileID.HoneySnowmanRocket || projectile.type == ProjectileID.DirtBomb
                || projectile.type == ProjectileID.DirtStickyBomb || projectile.type == ProjectileID.BombFish
                || projectile.type == ProjectileID.BombSkeletronPrime && (Main.getGoodWorld || Main.zenithWorld) || projectile.type == ProjectileID.Explosives
                || projectile.type == ModContent.ProjectileType<MiningDustProj>() || projectile.type == ModContent.ProjectileType<SuperBoom>() || projectile.type == ModContent.ProjectileType<SuperCreation>()
                || projectile.type == ModContent.ProjectileType<SuperBombProj_Brick>() || projectile.type == ModContent.ProjectileType<SuperBombProj_Brick_Sticky>()
                || projectile.type == ModContent.ProjectileType<SuperBombProj_Sticky>() || projectile.type == ModContent.ProjectileType<SuperBombProj_Bouncy>() || projectile.type == ModContent.ProjectileType<SuperBombProj>()
                 || projectile.type == ModContent.ProjectileType<MiningDustProj>())
                && projectile.position.Between(new Vector2(sx - (64 * 16), 50 * 16), new Vector2(sx + (64 * 16))))
            {
                projectile.timeLeft++;
                projectile.velocity *= 1.05f;
                if (projectile.scale < 0.9f)
                {
                    projectile.velocity *= 0.9f;
                    projectile.position = projectile.oldPosition;
                }
                projectile.tileCollide = false;
                projectile.scale -= 0.01f;
                if (projectile.scale < 0)
                    projectile.active = false;

                return false;
            }

            if (SnowArrow && owner2 != null && owner2.active && Timer % 2 == 0 && Timer > 2)
            {
                Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Main.rand.NextVector2Circular(1f, 1f), ModContent.ProjectileType<FrostMist>(), projectile.damage / 7, 0f, owner2.whoAmI);
            }

            if ((projectile.type == ProjectileID.PaladinsHammerHostile && owner != null && owner.type == NPCID.Paladin) && Downed.Contains(Bosses.Atmospheron))
            {
                if (Timer == 60)
                    projectile.velocity = projectile.DirectionTo(owner.Center) * 12f;
            }

            if ((projectile.type == ProjectileID.RocketSkeleton && owner != null && owner.type == NPCID.SkeletonCommando ||
                projectile.type == ProjectileID.Shadowflames && owner != null && owner.type == NPCID.GiantCursedSkull ||
                projectile.type == ProjectileID.BulletDeadeye && owner != null && owner.type == NPCID.TacticalSkeleton ||
                projectile.type == ProjectileID.ShadowBeamHostile && owner != null && Timer % 15 == 0 && Timer < 60 && (owner.type == NPCID.Necromancer || owner.type == NPCID.NecromancerArmored) ||
                projectile.type == ProjectileID.InfernoHostileBolt && (owner.type == NPCID.DiabolistRed || owner.type == NPCID.DiabolistWhite)) && Downed.Contains(Bosses.Atmospheron))
            {
                if (Timer % 2 == 0 && projectile.type == ProjectileID.RocketSkeleton)
                {
                    projectile.tileCollide = false;
                    Dust.NewDustPerfect(projectile.Center, ModContent.DustType<MuramasaDust>(), projectile.velocity * -1f, 0, default, 1f);
                }

                if (owner.type == NPCID.TacticalSkeleton)
                {
                    Dust d = Dust.NewDustPerfect(projectile.Center, DustID.CursedTorch, Vector2.Zero, 0, default, 2f);
                    d.noGravity = true;
                }

                float speed = projectile.type == ProjectileID.Shadowflames ? 3f : 6f;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player p = Main.player[i];
                    if (p != null && p.active && p.Distance(projectile.Center) < 500 && Timer > 30 && Timer < 120)
                    {
                        Vector2 vel = owner.type == NPCID.TacticalSkeleton ? Vector2.Lerp(projectile.velocity, projectile.DirectionTo(p.Center) * 8f, 0.05f) : projectile.DirectionTo(p.Center).RotatedByRandom(0.2f) * speed;
                        projectile.velocity = vel;
                    }
                }
            }

            if (owner2 != null && owner2.active && !projectile.hostile && projectile.type != ProjectileID.StardustGuardian && projectile.type != ModContent.ProjectileType<MuramasaSummon>() && owner2.HasBuff(ModContent.BuffType<SeraphTimeStop>()) || Main.LocalPlayer.HasBuff(ModContent.BuffType<SeraphTimeStop>()) && CanBeTimeStopped)
            {
                projectile.timeLeft = projectile.timeLeft + 1;
                if (PreTimeStopVel == Vector2.Zero) PreTimeStopVel = projectile.velocity;
                projectile.velocity = Vector2.Zero;
                return false;
            }
            else
            {
                if (PreTimeStopVel != Vector2.Zero)
                    projectile.velocity = PreTimeStopVel;
            }

            return base.PreAI(projectile);
        }
        public override bool? Colliding(Projectile projectile, Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player owner = Main.player[projectile.owner];
            float reference = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), owner.MountedCenter,
                projectile.Center, 1, ref reference) && projectile.ai[1] < 120 && (projectile.aiStyle == ProjAIStyleID.Yoyo
                && projectile.ModProjectile == null || projectile.type == ModContent.ProjectileType<PumpkinYoyoProj>()))
                return true;
            return base.Colliding(projectile, projHitbox, targetHitbox);    
        }
        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            return base.GetAlpha(projectile, lightColor);
        }
        public override void AI(Projectile projectile)
        {
            Player owner = Main.player[projectile.owner];

            switch (projectile.type)
            {
                case ProjectileID.PhantasmalDeathray: CustomCollision = true; break;
                case ProjectileID.SaucerDeathray: CustomCollision = true; break;
                case ProjectileID.HallowBossLastingRainbow: CustomCollision = true; break;
                case ProjectileID.FairyQueenSunDance: CustomCollision = true; break;
                case ProjectileID.HallowBossSplitShotCore: CustomCollision = true; break;
                case ProjectileID.VortexLaser: CustomCollision = true; break;
                case ProjectileID.VortexLightning: CustomCollision = true; break;
                case ProjectileID.StardustSoldierLaser: CustomCollision = true; break;
                case ProjectileID.SandnadoHostile: CustomCollision = true; break;
                case ProjectileID.DD2BetsyFlameBreath: CustomCollision = true; break;
                case ProjectileID.SolarFlareRay: CustomCollision = true; break;
                case ProjectileID.DeerclopsIceSpike: CustomCollision = true; break;
            }
        }

        public override void PostAI(Projectile projectile)
        {
            Timer++;

            base.PostAI(projectile);
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;


            if (FromMinigun)
            {
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;

                if (Timer > 5)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    TranscendenceUtils.DrawTrailProj(projectile, new Color(120, 15, 60), 2f, "TranscendenceMod/Miscannellous/Assets/Trail2", false, true, 1f, Vector2.Zero, true, 0);
                    TranscendenceUtils.DrawTrailProj(projectile, Color.Red, 1.75f, "TranscendenceMod/Miscannellous/Assets/Trail2", false, true, 1f, Vector2.Zero, true, 0);
                    TranscendenceUtils.DrawTrailProj(projectile, Color.Yellow, 1.5f, "TranscendenceMod/Miscannellous/Assets/Trail2", false, true, 1f, Vector2.Zero, true, 0);

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }

                return false;
            }

            if (FromSFtB)
            {
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 50;
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;

                TranscendenceUtils.DrawTrailProj(projectile, Color.Green * 0.75f, 2.5f, $"Terraria/Images/Projectile_977", false, true, 1.33f, Vector2.Zero, true, 0);
                TranscendenceUtils.DrawTrailProj(projectile, Color.Yellow, 2.125f, $"Terraria/Images/Projectile_977", false, true, 0.85f, Vector2.Zero, true, 0);

                return false;
            }
            if (projectile.aiStyle == ProjAIStyleID.Bobber && Main.player[projectile.owner].GetModPlayer<TranscendencePlayer>().UsingCrateMagnet)
            {
                TranscendenceUtils.DrawEntity(projectile, lightColor, 1, "TranscendenceMod/Miscannellous/Assets/CrateMagnetBobber", 0, projectile.Center + new Vector2(0, projectile.height * 0.75f), null);
            }
            return base.PreDraw(projectile, ref lightColor);
        }
    }
}
