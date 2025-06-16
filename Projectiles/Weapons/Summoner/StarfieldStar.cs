using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class StarfieldStar : ModProjectile
    {
        NPC victim;
        public int HomeCD;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 3;
            ProjectileID.Sets.MinionShot[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 640;
            Projectile.light = 0.7f;

            Projectile.width = 14;
            Projectile.height = 20;

            Projectile.penetrate = 1;
            Projectile.ArmorPenetration = 20;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.hostile = false;

            AIType = ProjectileID.Bullet;
        }
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.StardustPunch, Projectile.Center, Main.player[Projectile.owner].whoAmI);
            SoundEngine.PlaySound(SoundID.Item71 with { MaxInstances = 0, Volume = 0.2f}, Projectile.Center);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            victim = Projectile.FindTargetWithinRange(2400);

            if (++HomeCD > 65 && victim != null)
            {
                HomeCD = 0;
                Projectile.velocity = Projectile.DirectionTo(victim.Center) * 25;
            }
            if (Projectile.velocity == Vector2.Zero) Projectile.rotation = MathHelper.Pi;
            //Kill when idle
            if (++Projectile.ai[2] > 90 && Projectile.velocity == Vector2.Zero || Main.player[Projectile.owner].ownedProjectileCounts[Type] > 72)
                Projectile.Kill();
            //Main.NewText(Projectile.localAI[2]);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.velocity != Vector2.Zero)
                TranscendenceUtils.DrawTrailProj(Projectile, Color.Gray, 1f, Texture, false, true, 0.75f, Vector2.Zero);

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue * 0.5f, 1.25f / 5f, "TranscendenceMod/Miscannellous/Assets/GlowBloom", Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Blue * 0.5f, 1f / 5f, "TranscendenceMod/Miscannellous/Assets/GlowBloom", Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White * 0.5f, 0.66f / 5f, "TranscendenceMod/Miscannellous/Assets/GlowBloom", Projectile.rotation, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            /*SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];

                if (projectile != null && projectile.active && projectile.owner == Projectile.owner && projectile.type == Projectile.type && projectile.localAI[2] == projectile.localAI[2] && projectile.whoAmI != Projectile.whoAmI)
                {
                    sb.Draw(sprite, new Rectangle(
                        (int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), 16, (int)(Projectile.Distance(projectile.Center) * 2)), null,
                        Color.Blue, Projectile.DirectionTo(projectile.Center).ToRotation() + MathHelper.PiOver2,
                        sprite.Size() * 0.5f, SpriteEffects.None, 0);
                }
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);*/

            return base.PreDraw(ref lightColor);
        }
    }
}