using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class P2SupernovaBlackhole : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 3600;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.01f;
        }
        public override bool? CanDamage() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            BlackholeDrawer.DrawBlackhole(Projectile, Projectile.scale * 4f, spriteBatch);
            return false;
        }
        
        public override void AI()
        {
            if (Projectile.timeLeft < 90)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 90f);
            else if (Projectile.scale < 1f)
                Projectile.scale += 1f / 60f;

            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (npc == null) return;


            Player player = Main.player[npc.target];
            if (player == null) return;


            float changeSpeed = 0.0175f;
            float dist = 275f;

            Projectile.ai[0] += 0.5f;
            if (Projectile.ai[0] > 60)
                Projectile.ai[0] = 30;
            float speed2 = (float)Math.Sin(Projectile.ai[0]) * 140;

            Projectile.ai[2] += 0.25f;

            if (Projectile.timeLeft > 90)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center + Vector2.One.RotatedBy(Projectile.ai[2] * (float)Math.Tan(Projectile.ai[2] / 20)) * dist) * (90f + (speed2 * 0.66f)), changeSpeed);
            else Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 1f / 90f);


            if (player.Distance(Projectile.Center) < (175 * Projectile.scale))
            {
                player.velocity += player.DirectionTo(Projectile.Center) * 1.5f;
                player.AddBuff(ModContent.BuffType<BlackHoleDebuff>(), 5);
                Projectile.velocity *= 0.9f;
            }
        }
    }
}