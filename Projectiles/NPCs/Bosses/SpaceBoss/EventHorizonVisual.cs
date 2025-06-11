using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class EventHorizonVisual : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.timeLeft = 900;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];
            Vector2 targetVelocity = Projectile.DirectionTo(npc.Center + Vector2.One.RotatedByRandom(360) * Main.rand.Next(60, 150)) * 20;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.085f);

            if (Projectile.Distance(npc.Center) < 50)
                Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, TranscendenceWorld.BlackholeColor, 2f, "TranscendenceMod/Miscannellous/Assets/Circle", true, false, 1f, Vector2.Zero);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 1.5f, "TranscendenceMod/Miscannellous/Assets/Circle", true, false, 1f, Vector2.Zero);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}