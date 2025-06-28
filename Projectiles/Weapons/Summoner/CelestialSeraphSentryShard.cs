using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class CelestialSeraphSentryShard : ModProjectile
    {
        public bool HasHit;
        public NPC HITNPC;
        public Vector2 pos;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.MinionShot[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.light = 0.5f;

            Projectile.width = 34;
            Projectile.height = 34;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;

            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ArenaDust>(), Main.rand.NextFloat(), Main.rand.NextFloat(), 0, Color.White, 0.2f);
            }
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.timeLeft < 30)
                Projectile.ai[1] -= 1f / 30f;

            if (HITNPC != null)
            {
                Projectile.Center = HITNPC.Center + pos;
            }

            else
            {
                if (Projectile.ai[2] < 15)
                    Projectile.ai[1] += 1f / 15f;
                if (++Projectile.ai[2] > 45)
                {
                    NPC npc = Projectile.FindTargetWithinRange(2400);
                    if (npc == null)
                        return;

                    Projectile.velocity = Projectile.DirectionTo(npc.Center) * 15;
                }
            }


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!HasHit)
            {
                HITNPC = target;
                pos = Projectile.Center - target.Center;
                HasHit = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (HITNPC == null)
            {
                TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.5f, Projectile.scale * 0.5f, "TranscendenceMod/Miscannellous/Assets/GlowBloom", 0f, Projectile.Center, null);
                TranscendenceUtils.DrawEntity(Projectile, Color.White * 0.5f, Projectile.scale * 0.25f, "TranscendenceMod/Miscannellous/Assets/GlowBloom", 0f, Projectile.Center, null);
                TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, "TranscendenceMod/Miscannellous/Assets/StarEffect", true, true, 2f, Vector2.Zero);
            }
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, "TranscendenceMod/Miscannellous/Assets/StarEffect", 0f, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}