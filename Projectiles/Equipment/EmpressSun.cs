using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class EmpressSun : ModProjectile
    {
        public bool LacewingMode;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;

            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.timeLeft = 450;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Player player = Main.player[Projectile.owner];


            if (Main.dayTime)
            {
                ArtificialSun.DrawBreathingStar(Projectile, Projectile.scale * 0.075f, spriteBatch, true);
            }
            else
            {
                for (int i = 0; i < 7; i++)
                    TranscendenceUtils.DrawEntity(Projectile, Main.hslToRgb(i / 7f, 1f, 0.5f) * 0.75f, 1.375f * Projectile.scale, "bloom",
                    Projectile.direction, Projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 7f + Main.GlobalTimeWrappedHourly * 12f) * 6f * Projectile.scale, null);
                for (int i = 0; i < 4; i++)
                    TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, "bloom",
                        Projectile.direction, Projectile.Center, null);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            Projectile.velocity *= 0.9f;
            Projectile.damage = (int)(Projectile.damage * 0.9f);

            SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact with { MaxInstances = 0 }, target.Center);
            TranscendenceUtils.ParticleOrchestra(Main.dayTime ? ParticleOrchestraType.TrueExcalibur : ParticleOrchestraType.RainbowRodHit, target.Center, Projectile.owner);
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Projectile.scale = 0f;
            Projectile.ai[1] = Main.rand.NextFloat(0.0375f, 0.0675f);
            Projectile.ai[2] = 1f;

            if (Main.player[Projectile.owner].GetModPlayer<TranscendencePlayer>().LacewingTrans)
            {
                Projectile.ai[2] /= 2f;
                Projectile.width /= 2;
                Projectile.height /= 2;
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity.Y += 0.05f;

            if (Projectile.timeLeft < 120)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 120f);
            else if (Projectile.scale < 1f)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, Projectile.ai[2], 1f / 45f);

            if (Projectile.timeLeft > 300)
            {
                Projectile.ai[0] += Projectile.ai[1];
                Vector2 vec = Vector2.One.RotatedBy(Projectile.ai[0]) * (175f * Projectile.ai[2]);

                Projectile.Center = player.Center + new Vector2(vec.X / 2f, vec.Y).RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation());
            }

            float speed = 12f;
            if (Projectile.timeLeft == 299)
                Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * speed;
        }
    }
}