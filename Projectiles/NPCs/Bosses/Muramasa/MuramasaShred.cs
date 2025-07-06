using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Muramasa
{
    public class MuramasaShred : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 70;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.timeLeft = 90;
            Projectile.extraUpdates = 2;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(0.025f);

            if (Projectile.timeLeft < 35)
                ProjectileID.Sets.TrailCacheLength[Type] = Projectile.timeLeft * 2;
            else ProjectileID.Sets.TrailCacheLength[Type] = 70;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.DeepSkyBlue * 0.85f, 1.25f, $"Terraria/Images/Projectile_977", true, false, 2f, Vector2.Zero, true, MathHelper.PiOver2);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 0.5f, $"Terraria/Images/Projectile_977", true, false, 2f, Vector2.Zero, true, MathHelper.PiOver2);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}