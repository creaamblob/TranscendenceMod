using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class SunBeam : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public float RotSpeed;

        public float Height;
        public float Height2;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/BloomLine";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
            Projectile.scale = 0f;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 10;

            Player player = Main.player[Main.npc[(int)Projectile.ai[1]].target];

            if (player == null || !player.active)
                return;

            if (Projectile.ai[0] < 0)
                Projectile.ai[0]++;
            if (Projectile.ai[0] == -1)
                Projectile.ai[0] += 2;

            if (Projectile.ai[0] > 0)
            {
                Projectile.ai[0]++;

                if (Projectile.ai[0] == 2)
                {
                    Filters.Scene.Activate("TranscendenceMod:SaturationShader");
                    Filters.Scene["TranscendenceMod:SaturationShader"].GetShader().UseOpacity(1f);
                    Filters.Scene["TranscendenceMod:SaturationShader"].GetShader().UseProgress(6.25f);
                    Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().SaturationTimer = 10;

                    Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                        new Vector2(Main.rand.NextFloatDirection()), 15, 15, 15, -1, null));

                    SoundEngine.PlaySound(ModSoundstyles.SeraphBomb);
                }

                if (Height < 3000)
                    Height += 50f;

                if (Projectile.scale < 1f && Projectile.ai[0] < 15)
                    Projectile.scale += 0.1f;
            }
            else
            {
                if (Height2 < 750)
                    Height2 += 10f;
            }

            if (Projectile.ai[0] > 75f)
                Projectile.scale -= 1f / 15f;

            int timer = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer;
            if (timer < 75f)
                rot += MathHelper.Lerp(0.05f, 0f, timer / 75f) * Projectile.ai[2];

            if (Projectile.ai[0] > 90f)
                Projectile.Kill();
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;
            TranscendenceUtils.NoExpertProjDamage(Projectile);

            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Projectile.Center + Vector2.One.RotatedBy(rot) * 2000, (int)(15 * Projectile.scale), ref reference))
                return true;
            else return false;
        }
        public override bool? CanDamage() => Projectile.ai[0] > 20;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine3").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphMeteorStrike").Value;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * Height;
            Vector2 posB = Center + Vector2.One.RotatedBy(rot) * Height2;

            Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(rot) * (-30f - (Height / 20f));
            Vector2 pos2B = Projectile.Center + Vector2.One.RotatedBy(rot) * -100f;

            int timer = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer;
            float a = timer < 30 ? MathHelper.Lerp(0f, 1f, timer / 30f) : 1f;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack").Value;


            Projectile.localAI[2] += 0.025f;

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            
            TranscendenceUtils.DrawEntity(Projectile, Color.White * a, 1.6f * a, "TranscendenceMod/Miscannellous/Assets/GlowBloom", 0, Center, null);


            // Telegraph //
            sb.Draw(sprite2, new Rectangle(
                    (int)(posB.X - Main.screenPosition.X), (int)(posB.Y - Main.screenPosition.Y), (int)(120 * Height2 / 450f), (int)(posB.Distance(pos2B) * 2f)), null,
                    Color.Gold * a * 0.66f, posB.DirectionTo(pos2B).ToRotation() + MathHelper.PiOver2, sprite2.Size() * 0.5f, SpriteEffects.None, 0);
            // Telegraph //

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());

            eff.Parameters["useAlpha"].SetValue(true);
            eff.Parameters["alpha"].SetValue(Projectile.scale);

            eff.Parameters["useExtraCol"].SetValue(true);
            eff.Parameters["extraCol"].SetValue(new Vector3(1f, 0.875f, 0f));

            eff.Parameters["uTime"].SetValue(Projectile.localAI[2] * 0.5f);
            eff.Parameters["yChange"].SetValue(Projectile.localAI[2] * 0.75f);

            Main.instance.GraphicsDevice.Textures[1] = shaderImage;
            eff.Parameters["uImageSize1"].SetValue(new Vector2(shaderImage.Width * 0.375f, shaderImage.Height * 0.5f));

            // Laser //

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 8; i++)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(205 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}