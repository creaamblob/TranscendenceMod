using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class RoarShockwave : ModProjectile
    {
        public float Rad;
        public float MaxRad;
        public float Alpha = 1;
        public int Timer;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.timeLeft = 900;
            Projectile.penetrate = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            MaxRad = Projectile.timeLeft;
            Projectile.ai[2] = 125;
        }
        public override void AI()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2500;

            if (Projectile.ai[0] < 1f)
                Projectile.ai[0] += 1f / 30f;

            if (++Timer > 10 && Projectile.extraUpdates < 100)
            {
                Projectile.extraUpdates++;
                Timer = 0;
            }

            Projectile.ai[2] += (8 * (1 + (1 - Projectile.ai[0])));
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) > Projectile.ai[2])
                return Projectile.ai[0] >= 0.66f;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft >= 885)
                return false;

            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            Color col = Color.Lerp(Color.Black, Color.Red, Projectile.ai[0]) * Projectile.ai[0];

            string tex = TranscendenceMod.ASSET_PATH + "/Slash";

            for (int i = 0; i < 64; i++)
            {
                Vector2 pos = new Vector2(0, Projectile.ai[2] * (Projectile.ai[2] / 275f)).RotatedBy(MathHelper.TwoPi * i / 64f);
                float rot = (Projectile.Center + pos).DirectionTo(Projectile.Center).ToRotation() - MathHelper.PiOver2;
                float rot2 = (Projectile.Center + pos.RotatedBy(0.05f)).DirectionTo(Projectile.Center).ToRotation() - MathHelper.PiOver2;

                TranscendenceUtils.DrawEntity(Projectile, col, (1f + (Projectile.ai[2] / 200f)) * 0.5f, tex, rot2, Projectile.Center + pos.RotatedBy(0.05f), null);
                TranscendenceUtils.DrawEntity(Projectile, col * 0.5f, (1f + (Projectile.ai[2] / 200f)) * 2f, tex, rot, Projectile.Center + pos * 1.5f, null);
                TranscendenceUtils.DrawEntity(Projectile, col, 1f + (Projectile.ai[2] / 200f), tex, rot, Projectile.Center + pos, null);
            }

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);

            return false;
        }
    }
}