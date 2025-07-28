using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class SwordSlash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 55;
            Projectile.aiStyle = 1;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Stake;

            Projectile.width = 32;
            Projectile.height = 34;
            Projectile.timeLeft = 1200;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void PostDraw(Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, TranscendenceWorld.CosmicPurple * 0.33f, 1f,
            "TranscendenceMod/Miscannellous/Assets/Trail", 0, Projectile.Center + Projectile.velocity * 3, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.White * 0.33f, 0.75f,
            "TranscendenceMod/Miscannellous/Assets/Trail", 0, Projectile.Center + Projectile.velocity * 3, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            string sprite = $"{Texture}";
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.timeLeft > 1100 && Projectile.ai[1] != 1.85f)
            {
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 2f, "TranscendenceMod/Miscannellous/Assets/CelestialSeraphTelegraph",
                    Projectile.velocity.ToRotation(), Projectile.Center, null);
            }

            TranscendenceUtils.DrawTrailProj(Projectile, TranscendenceWorld.CosmicPurple, 1f, sprite, true, true, 2f, Vector2.Zero);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, sprite, Projectile.rotation, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void AI()
        {
            //if (Main.rand.NextBool(7)) Dust.NewDust(Projectile.Center, Projectile.width, 2, ModContent.DustType<NovaDust>(),
                //0, -10, 0, default, 0.7f);

            if (++Projectile.ai[2] > 45)
            {
                Projectile.extraUpdates += 1;
                Projectile.ai[2] = 0;
            }

            Projectile.spriteDirection = Projectile.direction;
        }
    }
}