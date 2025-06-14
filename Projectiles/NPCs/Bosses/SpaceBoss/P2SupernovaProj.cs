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
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/StarEffect";

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 3600;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (10f * Projectile.scale) && Projectile.active) return true;
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.OrangeRed, Projectile.scale * 0.5f,
                TranscendenceMod.ASSET_PATH + "/GlowBloom", 0, Projectile.Center, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.Yellow, Projectile.scale * 0.25f,
    TranscendenceMod.ASSET_PATH + "/GlowBloom", 0, Projectile.Center, null);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale,
                    Texture, 0, Projectile.Center, null);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);


            return false;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.99f;

            if (Projectile.timeLeft < 90)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 90f);

            if (Projectile.scale < 0.2f)
                Projectile.Kill();
        }
    }
}