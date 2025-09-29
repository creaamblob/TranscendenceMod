using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class StarBirthLaser : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public int Height;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
            Projectile.scale = 0f;
 
            Projectile.tileCollide = false;
            Projectile.scale = 1f;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            int time = (int)Projectile.ai[2];
            if (Projectile.timeLeft < (495 - time)) Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 100;
            for (float f = 0; f < 35; f++)
            {
                Lighting.AddLight(Vector2.SmoothStep(Center, Center + Vector2.One.RotatedBy(rot) * 1000, f / 35f), 0, 0.05f * Projectile.scale, 0.1f * Projectile.scale);
            }

            if (Projectile.timeLeft > (390 - time) && Projectile.timeLeft < (450 - time))
            {
                if (Height == 80)
                    SoundEngine.PlaySound(SoundID.Zombie104 with { MaxInstances = 0 });
                if (Height < 1260)
                    Height += 40;
            }

            if (Projectile.timeLeft < 25 && Projectile.scale > 0)
                Projectile.scale -= (0.4f * Projectile.ai[0]);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;
            Projectile.timeLeft = (int)(500 - Projectile.ai[2]);

            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().ModUnparryable = true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Projectile.Center + Vector2.One.RotatedBy(rot) * Height, Projectile.scale, ref reference))
                return Projectile.timeLeft >= 25;
            else return false;
        }
        public override bool? CanDamage() => Height > 180;
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine3").Value;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 50f;
            bool active = Height > 0;
            float am = active ? Height : 1250;
            Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(rot) * am;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack").Value;

            Projectile.localAI[2] += 0.0125f;

            if (!active)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), 15, (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * 0.66f, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }
            else
            {
                eff.Parameters["useAlpha"].SetValue(false);
                eff.Parameters["useExtraCol"].SetValue(true);
                eff.Parameters["extraCol"].SetValue(new Vector3(0f, 0.4f, 0.8f));

                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

                eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
                Main.instance.GraphicsDevice.Textures[1] = shaderImage;
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() / 2f);

                eff.Parameters["uTime"].SetValue(0);
                eff.Parameters["yChange"].SetValue(Projectile.localAI[2] * 2f);

                for (int i = 0; i < 8; i++)
                {
                    sb.Draw(sprite, new Rectangle(
                        (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(225 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                        Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
                }

                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                for (int i = 0; i < 3; i++)
                {
                    sb.Draw(sprite, new Rectangle(
                        (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(75 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                        Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
                }

                sb.End();
                sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}