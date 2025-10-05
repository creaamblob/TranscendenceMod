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

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class MeleeTornado : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 164;
            Projectile.height = 164;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = -1;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Projectile.scale = 0f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.White * 0.5f, Projectile.scale * 1.5f, Texture, Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => targetHitbox.Distance(Projectile.Center) < (96 * Projectile.scale);
        
        public override void AI()
        {
            if (Projectile.timeLeft < 20)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 20f);
            else
                Projectile.scale = MathHelper.Lerp(Projectile.scale, Projectile.timeLeft < 60 ? 2f : 1f, 1f / 90f);

            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (npc == null || !npc.active)
                Projectile.Kill();

            npc.Center = Projectile.Center;
            SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { MaxInstances = 0 }, Projectile.Center);

            Player player = Main.player[npc.target];
            if (player == null) return;


            float changeSpeed = 0.0225f * Projectile.scale;
            float dist = 50f;

            Projectile.ai[0] += 2f;
            if (Projectile.ai[0] >= 60)
                Projectile.ai[0] = 0;
            float speed2 = (float)Math.Sin(Projectile.ai[0]) * 250f;

            Projectile.ai[2] += 0.25f;
            Projectile.rotation += 0.1f * (Projectile.velocity.Length() / 10f);

            if (Projectile.timeLeft > 10)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center + Vector2.One.RotatedBy(Projectile.ai[2] * (float)Math.Tan(Projectile.ai[2] / 20f)) * dist) * ((225f * Projectile.scale) + speed2), changeSpeed);
            else Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 1f / 10f);

        }
    }
}