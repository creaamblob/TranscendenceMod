using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons
{
    public class ExoticRayBowTrailingShot : ModProjectile
    {
        public int HomeTimer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 3;
            Main.projFrames[Type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.timeLeft = 120;
            Projectile.penetrate = -1;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
             
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
        }
        public override void AI()
        {
            NPC target = Projectile.FindTargetWithinRange(500);

            if (target == null)
            {
                //if (++HomeTimer > 3) Projectile.Kill();
                return;
            }

            if (++HomeTimer % 15 == 0)
            {
                Projectile.velocity = Projectile.DirectionTo(target.Center) * 35;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 4);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            if (Projectile.velocity != Vector2.Zero)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, 3f, Texture, Projectile.rotation, Vector2.Zero, true, false, true);
                TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, 2.25f, Texture, Projectile.rotation, Vector2.Zero, true, false, true);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return false;
        }
    }
}