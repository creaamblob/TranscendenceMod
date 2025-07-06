using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class GalaxyShard : ModProjectile
    {
        //Projectiles are launched in Celestial Seraph Code
        public float Alpha;
        public bool Launched;
        public int Timer;
        public int Timer2;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 22;
            Projectile.height = 34;
            Projectile.timeLeft = 485;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 3000;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Projectile.ai[0] = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Main.getGoodWorld && !Main.zenithWorld && Projectile.ai[0] == 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                Rectangle rec = new Rectangle((int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), 8, 4000);
                Rectangle rec2 = new Rectangle((int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), 24, 4000);

                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine2").Value;
                Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine").Value;

                Main.spriteBatch.Draw(sprite, rec, null, Color.Blue * 0.75f, Projectile.rotation, sprite.Size() * 0.5f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(sprite2, rec2, null, new Color(20, 40, 170) * Alpha, Projectile.rotation, sprite2.Size() * 0.5f, SpriteEffects.None, 0);
                /*
                TranscendenceUtils.DrawEntity(Projectile, Color.Blue, 1.5f, "TranscendenceMod/Miscannellous/Assets/TelegraphLine", Projectile.rotation,
                    Projectile.Center, null);

                TranscendenceUtils.DrawEntity(Projectile, new Color(40, 90, 180) * Alpha, 1.5f, "TranscendenceMod/Miscannellous/Assets/BloomLine",
                    Projectile.rotation, Projectile.Center, null);
                */
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 1, $"{Texture}", true, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.NightsEdge, target.Center, target.whoAmI);
            SoundEngine.PlaySound(SoundID.DD2_EtherianPortalDryadTouch, target.Center);
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel.ToRotation() + MathHelper.PiOver2;
            if (Projectile.ai[0] == 1)
            {
                if (++Timer > 5)
                {
                    Projectile.velocity = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel;
                    Projectile.extraUpdates = 10;
                }
                Launched = true;    
            }
            else
            {
                Projectile.velocity *= 0.001f;
            }
            if (++Projectile.localAI[2] > 10)
                Alpha += 0.15f;
            if (Projectile.localAI[2] > 75)
                Alpha -= 0.2f;
            Projectile.spriteDirection = Projectile.direction;
        }
    }
}