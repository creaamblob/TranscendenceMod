using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Buffs.Items.Weapons;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class MuramasaSummon : ModProjectile
    {
        public int AttackTimer;
        public int attackdelay = 120;
        public Vector2 targetVelocity;
        public Vector2 dashPos;
        public float Fade;
        public int Resting;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            Projectile.friendly = true;
            Projectile.minionSlots = 1f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;

            Projectile.width = 24;
            Projectile.height = 76;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 3;
            Projectile.penetrate = -1;


            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 3;

        }

        public Color col => Color.Lerp(Color.White, new Color(0.5f, 0.5f, 0.5f, 1f), Projectile.ai[0] / 10f);
        public override Color? GetAlpha(Color lightColor) => col;

        private static VertexStrip _vertexStrip = new VertexStrip();
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Asset<Texture2D> sprite2 = TextureAssets.Extra[194];

            MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
            miscShaderData.UseImage1(sprite2);
            miscShaderData.UseSaturation(-2.25f);
            miscShaderData.UseOpacity(MathHelper.Lerp(0.5f, 0.375f, Projectile.ai[0] / 10f));
            miscShaderData.Apply();

            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2);
            _vertexStrip.DrawTrail();

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return true;
        }
        private Color StripColors(float progressOnStrip) => progressOnStrip < 0.05f ? Color.Transparent : Color.Lerp(Color.DeepSkyBlue, Color.Transparent, progressOnStrip * 0.5f);
        private float StripWidth(float progressOnStrip) => MathHelper.Lerp(32f, 0f, progressOnStrip * 0.5f);
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.ai[0] > 0f)
                behindProjectiles.Add(index);
            else overPlayers.Add(index);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.dead && player.HasBuff(ModContent.BuffType<MuramasaMinionBuff>()))
                Projectile.timeLeft = 3;

            ProjectileID.Sets.TrailCacheLength[Type] = 35;

            if (Resting > 0)
                Resting--;
            else Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.ai[2] > 0f)
                Projectile.ai[2] -= 1f;

            NPC npc = Projectile.FindTargetWithinRange(2250, false);

            if (npc != null && npc.active && player.Distance(Projectile.Center) < 2250 && !player.HasBuff(ModContent.BuffType<SeraphTimeStop>()))
            {
                Projectile.ai[0] = 0f;

                Projectile.velocity *= 0.99f;

                if (player.dead || !player.active)
                    return;

                Projectile.ai[2] = 60f;

                if (++AttackTimer < 5)
                {
                    targetVelocity = Projectile.DirectionTo(npc.Center) * 80f;
                }
                else
                {
                    if (AttackTimer < 15)
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity * (AttackTimer / 20f), 0.2f);
                    else Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[1]) * 0.9f;

                    if (AttackTimer > 25)
                    {
                        Projectile.velocity = Vector2.Zero;
                        Projectile.ai[1] = Main.rand.NextFloat(-0.175f, 0.175f);

                        targetVelocity = Projectile.DirectionTo(npc.Center) * 80f;

                        AttackTimer = 5;
                    }
                }
            }
            else
            {
                Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * Projectile.minionPos / player.ownedProjectileCounts[Type] + TranscendenceWorld.UniversalRotation * 2f) * 100f;
                vec.Y /= 3f;

                if (Projectile.ai[2] == 0f)
                {
                    Vector2 pos = player.Center + new Vector2(0, 20) + vec;

                    if (Projectile.Distance(pos) > 1000) Projectile.Center = pos;

                    if (Projectile.Distance(pos) > 60) Projectile.velocity = Projectile.DirectionTo(pos) * 20;
                    else
                    {
                        Projectile.rotation = MathHelper.Lerp(Projectile.rotation, MathHelper.Pi, 0.075f);

                        if (pos.Y < (player.Center.Y + 25))
                        {
                            if (Projectile.ai[0] < 10f)
                                Projectile.ai[0] += 1f;
                        }
                        else
                        {
                            if (Projectile.ai[0] > 0f)
                                Projectile.ai[0] -= 1f;
                        }

                        Projectile.Center = pos;

                        Projectile.position.Y += (float)(Math.Sin((TranscendenceWorld.UniversalRotation + Projectile.minionPos) * 3f) * 20f);

                        Resting = 5;
                        Projectile.velocity *= 0.9f;
                    }
                    AttackTimer = 0;
                }
            }
        }
    }
}