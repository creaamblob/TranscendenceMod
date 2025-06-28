using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class StarGlyphNoReturn : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/SpaceBoss/StarGlyph";
        public float Alpha = 1;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 240;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White * Alpha;
        public override void AI()
        {
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 3);
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            if (Projectile.ai[0] != 3) TranscendenceUtils.DustRing(Projectile.Center, 10, DustID.BlueFairy, 3, Color.White, 1.25f);
        }
        public override bool CanHitPlayer(Player target) => Alpha == 1;
        public override bool PreDraw(ref Color lightColor)
        {
            /*SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.75f, 0.5f, "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG", 0, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);*/

            for (int i = 0; i < 4; i++)
            {
                float pi = MathHelper.TwoPi * i / 4;
                float rot = pi += MathHelper.ToRadians(++Projectile.localAI[2] + 4);

                Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(rot) * 10;

                TranscendenceUtils.DrawProjAnimated(Projectile, Color.DeepSkyBlue * Alpha * 0.45f, Projectile.scale * 2,
                    $"{Texture}", Projectile.rotation, pos, false, false, false);
            }
            return true;
        }
    }
}