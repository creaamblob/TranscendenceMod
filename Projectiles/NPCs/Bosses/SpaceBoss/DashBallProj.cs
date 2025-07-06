using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class DashBallProj : ModProjectile
    {
        public int Timer;
        public Vector2 startPos;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 120;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            AIType = ProjectileID.Bullet;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;

            startPos = Projectile.Center;
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            SoundEngine.PlaySound(SoundID.Item33 with { MaxInstances = 0 }, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 10)
            {
                Texture2D sprite4 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Trail2").Value;
                Vector2 origin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

                SpriteBatch sb = Main.spriteBatch;

                //Request the Effect
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
                //Apply Shader Texture
                string string1 = "TranscendenceMod/Miscannellous/Assets/ModifierEffect";
                Texture2D shaderImage = ModContent.Request<Texture2D>(string1).Value;

                Projectile.localAI[2] += 0.0125f;

                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

                eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
                eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 0.3f);
                Main.instance.GraphicsDevice.Textures[1] = shaderImage;

                eff.Parameters["uTime"].SetValue(Projectile.localAI[2] * 1.75f);
                eff.Parameters["yChange"].SetValue(Projectile.localAI[2] * 1.75f);

                eff.Parameters["useExtraCol"].SetValue(true);
                eff.Parameters["extraCol"].SetValue(new Vector3(2f, 0f, 1f));

                eff.Parameters["useAlpha"].SetValue(false);

                for (int i = 3; i < ((Projectile.oldPos.Length / 2)); i++)
                {
                    Vector2 pos = Projectile.oldPos[i * 2] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);

                    Main.EntitySpriteDraw(sprite4, pos, null, Color.White, Projectile.oldRot[i * 2] + MathHelper.PiOver2, sprite4.Size() * 0.5f, Projectile.scale * 3f, SpriteEffects.None);
                }

                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                for (int i = 6; i < ((Projectile.oldPos.Length - 2)); i++)
                {
                    Vector2 pos = Projectile.oldPos[i] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);

                    Main.EntitySpriteDraw(sprite4, pos, null, Color.White, Projectile.oldRot[i] + MathHelper.PiOver2, sprite4.Size() * 0.5f, Projectile.scale * 2f, SpriteEffects.None);
                }

                sb.End();
                sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            else
            {
                TranscendenceUtils.DrawEntity(Projectile, Color.Magenta, 12.5f, "bloom", MathHelper.ToRadians(Projectile.ai[2] * 3), Projectile.Center, null);
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 8, "bloom", MathHelper.ToRadians(Projectile.ai[2] * 3), Projectile.Center, null);
            }

            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (targetHitbox.Distance(Projectile.oldPos[i]) < 6 && Projectile.localAI[1] > 15 || Projectile.timeLeft < 10 && targetHitbox.Distance(Projectile.Center) < 100)
                    return true;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.localAI[1]++;
            Projectile.spriteDirection = (int)Projectile.velocity.ToRotation();

            if (Projectile.timeLeft == 10)
            {
                SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 5 }, Projectile.Center);
            }

            if (++Timer > 1)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (++Projectile.ai[2] < 20 ? 90 : 25)) 
                    * (Projectile.ai[2] > 20 ? 1.015f : 1);
                Timer = 0;
            }
            /*if (Projectile.velocity == Vector2.Zero && Projectile.timeLeft < 875)
                Projectile.Kill();*/
        }
    }
}