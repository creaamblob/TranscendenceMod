using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent
{
    public class HomingFrostBlast : ModProjectile
    {
        public int Timer;
        public int Timer2;
        public Vector2 pos;
        public float Speed = 0;
        public Player player;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.tileCollide = false;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 240;
            Projectile.hostile = true;

            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2500;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Speed < 55)
                Speed += 2.5f;

            Timer2++;
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player2 = Main.player[p];

                if (player2 != null)
                {
                    player = player2;
                    break;
                }
            }
            if (player != null && player.active)
            {
                if (player.Distance(Projectile.Center) < 5000 && Timer2 >= 45 && Timer2 < 150)
                {
                    if (Timer2 % 3 == 0)
                    {
                        pos = player.Center + Vector2.One.RotatedBy(Projectile.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver4) * 1500;
                    }
                    Vector2 targetVelocity = Projectile.DirectionTo(pos + Vector2.One.RotatedByRandom(360) * Main.rand.Next(25, 100)) * Speed;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.0375f);
                    Timer = 0;
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            float alpha = Timer2 > 40 ? MathHelper.Lerp(1f, 0f, (Timer2 - 40) / 30f) : 1f;
            if (player != null && player.active)
            {
                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

                spriteBatch.Draw(sprite, new Rectangle(
                        (int)(Projectile.Center.X - Main.screenPosition.X),
                        (int)(Projectile.Center.Y - Main.screenPosition.Y),
                        25,
                        (int)(Projectile.Distance(player.Center) * 2)), null,
                        Color.DeepSkyBlue * 0.5f * alpha, Projectile.DirectionTo(player.Center).ToRotation() + MathHelper.PiOver2,
                        sprite.Size() * 0.5f,
                        SpriteEffects.None,
                        0);

                TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.75f * alpha, 0.5f, "TranscendenceMod/Miscannellous/Assets/SeraphAura", MathHelper.ToRadians(Timer2 * 2f), player.Center, null);
            }

            TranscendenceUtils.DrawTrailProj(Projectile, Color.Blue * 0.33f, Projectile.scale * 3f, "TranscendenceMod/Miscannellous/Assets/Trail3",
                true, true, 1f, Vector2.Zero, true, MathHelper.PiOver2);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.DeepSkyBlue, Projectile.scale, "TranscendenceMod/Miscannellous/Assets/Trail3",
                true, true, 1f, Vector2.Zero, true, MathHelper.PiOver2);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}