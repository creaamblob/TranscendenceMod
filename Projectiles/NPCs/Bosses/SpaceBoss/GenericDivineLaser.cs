using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class GenericDivineLaser : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public float RotSpeed;
        public float Height;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/WaterBeam";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
            Projectile.scale = 0f;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 10;

            Player player = Main.player[Main.npc[(int)Projectile.ai[1]].target];

            if (player == null || !player.active)
                return;

            if (Projectile.ai[0] < 0)
                Projectile.ai[0]++;
            if (Projectile.ai[0] == -1)
                Projectile.ai[0] += 2;

            if (Projectile.ai[0] > 0)
            {
                Projectile.ai[0]++;

                if (Projectile.localAI[1] != 1)
                {
                    SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 });
                    Projectile.localAI[1] = 1;
                }

                if (Height < 3000)
                    Height += 100f;

                if (Projectile.scale < 1f && Projectile.ai[0] < 15)
                    Projectile.scale += 0.1f;
            }

            if (Projectile.ai[0] > 75f)
            {
                Projectile.scale -= 1f / 15f;
                Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 0f, 1f / 15f);
            }

            if (Projectile.ai[0] > 90f)
                Projectile.Kill();
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;
            TranscendenceUtils.NoExpertProjDamage(Projectile);

            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Projectile.Center + Vector2.One.RotatedBy(rot) * 3000, (int)(10 * Projectile.ai[2] * Projectile.scale), ref reference))
                return true;
            else return false;
        }
        public override bool? CanDamage() => Projectile.ai[0] > 20;
        public override bool PreDraw(ref Color lightColor)
        {
            DivineBeam.DrawDivineLaser(Projectile, Projectile.ai[0] < 2, (int)(40 * Projectile.ai[2]), Center, rot, Height);
            return false;
        }
    }
}