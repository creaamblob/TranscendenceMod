using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class CosmicKick : ModProjectile
    {
        public int Timer;

        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 pos = Vector2.One.RotatedBy(TranscendenceWorld.UniversalRotation * 7) * 100;
            Projectile.Center = player.Center + new Vector2(pos.X, pos.Y / 2);

            Projectile.timeLeft = 60;

            if (!player.GetModPlayer<TranscendencePlayer>().CosmicSetWear || player.dead)
                Projectile.Kill();

            Projectile.spriteDirection = player.direction;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 80;
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.DeepSkyBlue * 0.75f, 1f, "TranscendenceMod/Miscannellous/Assets/OldGlowBloom", false, true, 1f, Vector2.Zero);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White * 0.75f, 0.5f, "TranscendenceMod/Miscannellous/Assets/OldGlowBloom", false, true, 1f, Vector2.Zero);

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return true;
        }
    }
}