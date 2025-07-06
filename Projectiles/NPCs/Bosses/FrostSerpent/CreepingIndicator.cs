using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent
{
    public class CreepingIndicator : ModProjectile
    {
        public float Fade;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/SeraphAura2";
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.extraUpdates = 1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Fade = 1f;
        }
        public override bool CanHitPlayer(Player target)
        {
            return Fade > 0.85f && Projectile.scale > 0.9f;
        }
        public override void AI()
        {
            if (++Projectile.ai[2] > 30)
                Projectile.velocity *= 0.95f;
            
            if (Projectile.timeLeft < 45 && Fade > 0f)
                Fade -= 1f / 45f;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * 6) * 2f;
                TranscendenceUtils.DrawEntity(Projectile, Color.Blue * 0.66f, Fade / 2f, Texture, 0, pos, null);
            }
            TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue, Fade / 2f, Texture, 0, Projectile.Center, null);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}