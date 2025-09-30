using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class TornadoHostile : ModProjectile
    {
        Rectangle rec;
        public float Opacity = 0;
        public float Rotation;
        public override string Texture => "TranscendenceMod/Projectiles/NPCs/Bosses/Dragon/TornadoTelegraph";

        public override void SetDefaults()
        {
            Main.projFrames[Type] = 3;

            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.timeLeft = 90;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Rotation += 0.15f;

            if (Projectile.ai[2] > 60)
            {
                for (int i = 0; i < (32 * (Projectile.ai[0] + 1)); i++)
                {
                    SpriteBatch sb = Main.spriteBatch;
                    sb.End();
                    sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    TranscendenceUtils.DrawEntity(Projectile, Color.White * Opacity, 1 + (i * 0.035f), $"Terraria/Images/Projectile_657", Rotation,
                        new Vector2(Projectile.Center.X, Projectile.Center.Y - (i * 5)), null);

                    sb.End();
                    sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
            else
            {
                TranscendenceUtils.AnimateProj(Projectile, 4);

                for (int i = 0; i < 6; i++)
                {
                    TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, Texture, 0f, Projectile.Center - new Vector2(0, i * 55f), false, false, false);
                }
            }
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void AI()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;
            

            rec = new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y - (148 *
                ((int)Projectile.ai[0] + 1)), 28, 148 * ((int)Projectile.ai[0] + 1));

            if (Projectile.ai[2] > 60 && Opacity < 0.5f)
                Opacity += 1f / 60f;

            if (Projectile.ai[0] < 1)
                Projectile.ai[0] += 0.025f;

            Projectile.ai[2]++;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Intersects(rec) && Opacity > 0.33f && Projectile.ai[2] > 90)
                return true;
            else
                return false;
        }
    }
}