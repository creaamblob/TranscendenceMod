using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class PlayerGameTrans : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloom";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 192;
            Projectile.height = 192;

            Projectile.aiStyle = -1;

            Projectile.timeLeft = 600;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = false;
            Projectile.hide = true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];

            if (++Projectile.ai[2] > 30)
            {
                Vector2 targetVelocity = Projectile.DirectionTo(npc.Center + new Vector2(0, 48)) * 14f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.085f);
            }

            if (Projectile.Distance(npc.Center + new Vector2(0, 48)) < 25)
                Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.Red, 0.25f, "TranscendenceMod/Miscannellous/Assets/GlowBloom", false, true, 1f, Vector2.Zero);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 0.125f, "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG", false, true, 1f, Vector2.Zero);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}