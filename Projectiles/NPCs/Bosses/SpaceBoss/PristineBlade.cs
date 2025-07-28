using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class PristineBlade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 72;

            Projectile.timeLeft = 900;

            Projectile.extraUpdates = 1;
            Projectile.light = 0.75f;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < 15 && Projectile.localAI[2] > 5)
                return true;
            else return false;
        }

        public float rot;

        public override void OnSpawn(IEntitySource source)
        {
            rot = Projectile.ai[0];
            Projectile.ai[0] = 275f;

            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];

            if (npc == null)
                return;

            Player player = Main.player[npc.target];

            if (player == null || !player.active || player.dead)
                Projectile.active = false;

            if (Projectile.localAI[2] == 2)
            {
                SoundEngine.PlaySound(SoundID.Item162 with { MaxInstances = 0 }, Projectile.Center);
                for (int i = 0; i < 32; i++)
                {
                    Vector2 vel = new Vector2(0, 7.5f + (float)Math.Sin(i) * 1.5f).RotatedBy(MathHelper.TwoPi * i / 32f + MathHelper.PiOver4 / 2f + Projectile.rotation);
                    Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<PlayerCosmicBlood>(), vel, 0, Color.Lerp(Color.Gold, Color.SaddleBrown, i / 32f), 2f);
                }
            }

            if (++Projectile.localAI[2] == 90)
            {
                Projectile.velocity = Projectile.DirectionTo(player.Center) * 12f;
                for (int i = 0; i < 3; i++)
                    SoundEngine.PlaySound(SoundID.Item71 with { MaxInstances = 0 }, Projectile.Center);
            }

            if (Projectile.localAI[2] > 90)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                Projectile.velocity *= 1.05f;
            }
            else
            {
                if (Projectile.localAI[2] >= 45)
                    Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 0f, 1f / 20f);

                rot += 0.0125f * Projectile.ai[2];

                if (Projectile.localAI[2] > 60)
                    Projectile.ai[0] += 2;

                Projectile.Center = player.Center + Vector2.One.RotatedBy(rot) * Projectile.ai[0];
                Projectile.rotation = Projectile.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2;
            }


        }
        public override bool PreDraw(ref Color lightColor)
        {
            float fade = Projectile.localAI[2] < 45 ? Projectile.localAI[2] / 45f : 1f;

            if (Projectile.localAI[2] > 90)
            {
                SpriteBatch spriteBatch = Main.spriteBatch;
                TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

                TranscendenceUtils.DrawTrailProj(Projectile, Color.Gold * 0.75f, Projectile.scale * 2f, "TranscendenceMod/Miscannellous/Assets/Trail2", false, true, 1f, Vector2.Zero);
                TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, "TranscendenceMod/Miscannellous/Assets/Trail2", false, true, 1f, Vector2.Zero);

                TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            }

            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 1f, 0.75f, 0f, 0.5f * fade, false);
            TranscendenceUtils.DrawEntity(Projectile, Color.White * fade, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}