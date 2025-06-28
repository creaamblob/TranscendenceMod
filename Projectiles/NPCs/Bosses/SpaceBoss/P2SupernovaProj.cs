using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class P2SupernovaProj : ModProjectile
    {
        public Projectile proj;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/StarEffect";

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 3600;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Projectile.ai[0] = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (10f * Projectile.scale) && Projectile.active) return true;
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            float a = Projectile.ai[2] < 60 ? Projectile.ai[2] / 60f : 1f;


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.OrangeRed * a, Projectile.scale * 0.5f,
                TranscendenceMod.ASSET_PATH + "/GlowBloom", 0, Projectile.Center, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.Yellow * a, Projectile.scale * 0.25f,
                TranscendenceMod.ASSET_PATH + "/GlowBloom", 0, Projectile.Center, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.White * a, Projectile.scale,
                    Texture, 0, Projectile.Center, null);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);


            return false;
        }
        public override bool? CanDamage() => Projectile.ai[2] > 120;
        public override void AI()
        {
            Projectile.velocity *= 0.99f;
            Projectile.ai[2]++;

            if (Projectile.timeLeft < 60)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 60f);

            if (Projectile.scale < 0.2f)
                Projectile.Kill();

            if (Projectile.ai[2] > 100)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, new Vector2(0, 3.75f).RotatedBy(Projectile.ai[0]), 1f / 90f).RotatedBy(0.025f);
            }
        }
    }
}