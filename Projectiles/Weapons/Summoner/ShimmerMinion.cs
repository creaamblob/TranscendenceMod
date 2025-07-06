using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Weapons;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class ShimmerMinion : ModProjectile
    {
        public int AttackTimer;
        public int attackdelay = 120;
        public int ShotCounter;
        public Vector2 targetVelocity;
        public bool Host;
        public bool Active;
        public int GuardCD;
        public int GuardMaxCD = 900;
        public int ShieldDura;
        public int ShieldMaxDura = 7;
        public ShimmerMinion minion;
        public Projectile HostProjectile;
        public bool Behind;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 3;
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;

            Projectile.minionSlots = 0.16666666666666666666666666666667f;

            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 3;
            Projectile.penetrate = -1;
        }

        public override bool? CanDamage() => true;

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Behind)
                behindProjectiles.Add(index);
            else overPlayers.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Host)
            {
                TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.75f, 3f, "bloom", 0, Projectile.Center, null);
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 2.5f, "bloom", 0, Projectile.Center, null);
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 1.75f, "bloom", 0, Projectile.Center, null);
                return false;
            }
            else
            {
                SpriteBatch spriteBatch = Main.spriteBatch;
                Color orange = Projectile.ai[2] > 0 ? Color.SaddleBrown * 0.66f : Color.Orange * 2f;
                Color blue = Projectile.ai[2] > 0 ? Color.RoyalBlue * 0.66f : Color.DeepSkyBlue * 2f;
                float alpha = Projectile.ai[0] == 0 ? 1f : 1f - ((Projectile.localAI[2] - 30) / (float)GuardMaxCD);
                Color col = (Projectile.minionPos % 2 == 0 ? orange : blue) * alpha;

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

                int amount = Main.player[Projectile.owner].ownedProjectileCounts[Type] - 2;

                if (minion != null && minion.Active)
                {
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p != null && p.active && p.minionPos != 0 && Projectile.minionPos < (amount + 1) && p.minionPos == (Projectile.minionPos - 5) && p.type == Type)
                        {
                            spriteBatch.Draw(sprite, new Rectangle(
                                (int)(Projectile.Center.X - Main.screenPosition.X),
                                (int)(Projectile.Center.Y - Main.screenPosition.Y),
                                48,
                                (int)(Projectile.Distance(p.Center) * 2f)), null,
                                Color.Gray * 0.75f, Projectile.DirectionTo(p.Center).ToRotation() + MathHelper.PiOver2,
                                sprite.Size() * 0.5f,
                                SpriteEffects.None,
                                0);
                        }
                    }
                }

                if (Projectile.ai[0] == 1)
                {
                    Vector2 position = Main.player[Projectile.owner].Center;
                    if (Projectile.minionPos % 2 == 0)
                    {
                        spriteBatch.Draw(sprite, new Rectangle(
                            (int)(Projectile.Center.X - Main.screenPosition.X),
                            (int)(Projectile.Center.Y - Main.screenPosition.Y),
                            15,
                            (int)(Projectile.Distance(position) * 2f)), null,
                            Color.Gray * 0.66f * alpha, Projectile.DirectionTo(position).ToRotation() + MathHelper.PiOver2,
                            sprite.Size() * 0.5f,
                            SpriteEffects.None,
                            0);
                    }

                    if (Projectile.minionPos < amount && Projectile.minionPos > (amount - 5) || Projectile.minionPos == amount)
                    {
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            Projectile p = Main.projectile[i];
                            if (p != null && p.active && (Projectile.minionPos != amount && p.minionPos == (Projectile.minionPos + 1) || Projectile.minionPos == amount && p.minionPos == (Projectile.minionPos - 4)) && p.type == Type)
                            {
                                spriteBatch.Draw(sprite, new Rectangle(
                                    (int)(Projectile.Center.X - Main.screenPosition.X),
                                    (int)(Projectile.Center.Y - Main.screenPosition.Y),
                                    24,
                                    (int)(Projectile.Distance(p.Center) * 2f)), null,
                                    (Projectile.ai[2] > 0 ? Color.Blue : Color.DeepSkyBlue) * alpha, Projectile.DirectionTo(p.Center).ToRotation() + MathHelper.PiOver2,
                                    sprite.Size() * 0.5f,
                                    SpriteEffects.None,
                                    0);
                                spriteBatch.Draw(sprite, new Rectangle(
                                    (int)(Projectile.Center.X - Main.screenPosition.X),
                                    (int)(Projectile.Center.Y - Main.screenPosition.Y),
                                    6,
                                    (int)(Projectile.Distance(p.Center) * 2f)), null,
                                    Color.White * alpha, Projectile.DirectionTo(p.Center).ToRotation() + MathHelper.PiOver2,
                                    sprite.Size() * 0.5f,
                                    SpriteEffects.None,
                                    0);
                            }
                        }
                    }
                }

                Texture2D star = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/StarEffect").Value;
                DrawData drawData = new DrawData(star, Projectile.Center - Main.screenPosition, null, col, 0, star.Size() * 0.5f, 2.25f, SpriteEffects.None);
                GameShaders.Armor.Apply(Main.player[Projectile.owner].cWings, Projectile, drawData);
                drawData.Draw(spriteBatch);

                TranscendenceUtils.DrawEntity(Projectile, col, 1.85f, "TranscendenceMod/Miscannellous/Assets/StarEffect",
                    0, Projectile.Center, null);

                TranscendenceUtils.DrawEntity(Projectile, Color.White * alpha, 1.5f, "TranscendenceMod/Miscannellous/Assets/StarEffect",
                    0, Projectile.Center, null);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                return false;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];

            if (player.ownedProjectileCounts[Type] == 0 && Projectile.ai[1] == 0)
            {
                Host = true;
                Active = true;
                GuardCD = GuardMaxCD;
                Projectile.localNPCHitCooldown = 20;
            }

            if (Projectile.ai[1] == 0 && player.slotsMinions < player.maxMinions)
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), Projectile.Center, Vector2.Zero, Type, Projectile.damage, Projectile.knockBack, player.whoAmI, 0, 1);
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            float mult = MathHelper.Lerp(1f, 2.5f, Main.player[Projectile.owner].ownedProjectileCounts[Type] / 50f);
            modifiers.FinalDamage *= mult;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.dead && player.HasBuff(ModContent.BuffType<SpaceBossMinions>()))
                Projectile.timeLeft = 3;

            Behind = true;
            int count = Projectile.minionPos == player.ownedProjectileCounts[Type] ? 0 : Projectile.minionPos == player.ownedProjectileCounts[Type] - 1 ? 1 : Projectile.minionPos;

            if (Projectile.ai[2] > 0)
                Projectile.ai[2]--;

            if (GuardCD > 0)
                GuardCD--;

            if (GuardCD > 1 && GuardCD < 10)
                ShieldDura = ShieldMaxDura;

            int target = Projectile.FindTargetWithLineOfSight(1500);
            bool Idle = target == -1 || player.Distance(Projectile.Center) > 1000 || !Active;

            if (Active && !Idle)
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, 0f, 0.05f);

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (p != null && p.type == Projectile.type && p.active && p.ModProjectile is ShimmerMinion minion2 && minion2.Host && !Host && HostProjectile == null)
                {
                    minion = minion2;
                    HostProjectile = p;
                }

                if (Projectile.ai[0] == 1 && minion != null && minion.GuardCD == 0 && !minion.Active)
                {
                    if (p != null && p.active && p.hostile && p.Hitbox.Intersects(Projectile.Hitbox) && p.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), p.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(),
                            350, 35, -1, 0f, 0.8f, 1f);
                        p.Kill();
                        SoundEngine.PlaySound(new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Hurt/CelestialSeraphShield")
                        {
                            Volume = 1.2f,
                            Pitch = 2f,
                            MaxInstances = 0
                        }, HostProjectile.Center);
                        if (minion.ShieldDura > 0)
                            minion.ShieldDura--;
                        else minion.GuardCD = GuardMaxCD;
                    }
                }
            }

            if (!Host && HostProjectile != null && HostProjectile.active)
            {
                int amount = Main.player[Projectile.owner].ownedProjectileCounts[Type] - 2;

                bool edges = (Projectile.minionPos < amount && Projectile.minionPos > (amount - 5) || Projectile.minionPos == amount) && !minion.Active;

                int am = (int)(MathHelper.Lerp(count, count * 1.5f, Projectile.minionPos / (float)count));
                Vector2 vec = Vector2.One.RotatedBy(TranscendenceWorld.UniversalRotation * 2 + (MathHelper.TwoPi * count / 5) + MathHelper.ToRadians((int)(am / (minion.Active ? 2 : 4)) * 5)) * (25 + (int)(count / (edges ? 4 : 6)) * (!minion.Active ? 17.5f : 25f));

                Behind = minion.Behind;
                Projectile.localAI[2] = MathHelper.Lerp(Projectile.localAI[2], minion.GuardCD, 0.1f);
                Projectile.Center = HostProjectile.Center + new Vector2(vec.X, vec.Y / (minion.Active ? 3f : 4.5f)).RotatedBy(HostProjectile.rotation);

                if (minion.GuardCD > 2 && minion.GuardCD < 5)
                {
                    TranscendenceUtils.DustRing(Projectile.Center, 10, ModContent.DustType<ArenaDust>(), 4, Projectile.ai[2] > 0 ? Color.RoyalBlue : Color.DeepSkyBlue, 0.75f);
                    SoundEngine.PlaySound(SoundID.MaxMana, HostProjectile.Center);
                }

                if (!minion.Active)
                    Projectile.ai[0] = 1;
                else Projectile.ai[0] = 0;

                if (Projectile.Center.Y < HostProjectile.Center.Y)
                    Projectile.ai[2] = 5;
            }

            if (!Host)
                return;

            if (player.altFunctionUse == 2 && player.ItemAnimationJustStarted)
                Active = !Active;

            float dist = Projectile.Distance(player.Center);
            float chaseSpeed = (dist > 2000 ? 150 : dist > 400 ? 50 : 10);

            if (!Idle && target != -1)
            {
                NPC npc = Main.npc[target];
                if (npc == null || !npc.active)
                    Idle = true;

                if (player.dead || !player.active)
                    return;

                if (++AttackTimer > 40)
                {
                    targetVelocity = Projectile.DirectionTo(npc.Center + Vector2.One.RotatedBy(TranscendenceWorld.UniversalRotation * -2f) * 25f) * 50f;
                    float accuracy = 0.025f + (player.ownedProjectileCounts[Type] / 240f);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, accuracy);
                    Projectile.velocity = Projectile.velocity.RotatedBy(0.175f);
                    Projectile.rotation = MathHelper.Lerp(Projectile.rotation, Projectile.velocity.ToRotation() + MathHelper.PiOver4, 0.025f);
                    if (AttackTimer > 150) AttackTimer = 0;
                }
                else
                {
                    float rot = player.DirectionTo(npc.Center).ToRotation();
                    targetVelocity = Projectile.DirectionTo(npc.Center + Vector2.One.RotatedBy(rot) * 150f) * 20f;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.1f);
                    Projectile.rotation = rot;

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, 2.5f).RotatedBy(TranscendenceWorld.UniversalRotation * 16f), ModContent.ProjectileType<CelestialSeraphSentryShard>(),
                        (int)(Projectile.damage * 1.75f), 2f, player.whoAmI);
                }
            }
            else
            {
                if (Active)
                {
                    Vector2 vec = Vector2.One.RotatedBy(TranscendenceWorld.UniversalRotation * 4f) * 175f;
                    Vector2 pos = player.Center + new Vector2(vec.X, vec.Y / 2f);
                    Vector2 targetVelocity = Projectile.DirectionTo(pos);
                    if (Projectile.Distance(pos) > 50) Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity * chaseSpeed, 0.075f);
                    else Projectile.velocity *= 0.9f;
                    Projectile.rotation = Projectile.velocity.X * 0.025f;
                }
                else
                {
                    Projectile.localAI[2] = player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver2 + MathHelper.PiOver4;
                    Vector2 pos = player.Center + Vector2.One.RotatedBy(Projectile.localAI[2]) * (55f + (12.5f * count));
                    Projectile.Center = Vector2.Lerp(Projectile.Center, pos, 0.2f);
                    Projectile.velocity *= 0.9f;
                    Projectile.rotation = player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver2;

                    Behind = pos.Y > player.Bottom.Y;
                }
            }
        }
    }
}