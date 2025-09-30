using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class DivineBeam : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/WaterBeam";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
            Projectile.scale = 0f;
 
            Projectile.tileCollide = false;
            Projectile.timeLeft = 110;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.ai[2]++;

            Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 10;

            Player player = Main.player[Main.npc[(int)Projectile.ai[1]].target];
            if (player == null || !player.active)
                return;


            if (Projectile.ai[2] < 65)
            {
                if (Projectile.ai[2] < 55)
                    Center.X = Center.MoveTowards(player.position + player.velocity * 28, 65).X;
                Center.Y = player.position.Y - 1000;
                Projectile.ai[0] += 15;
            }

            if (Projectile.ai[2] == 65)
            {
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                    new Vector2(Main.rand.NextFloatDirection()), 15, 5, 5, -1, null));
                SoundEngine.PlaySound(SoundID.Zombie104 with { MaxInstances = 0});
            }

            if (Projectile.ai[2] > 60 && Projectile.timeLeft > 20 && Projectile.scale < 1f)
                Projectile.scale += 0.05f;

            if (Projectile.timeLeft < 20)
                Projectile.scale -= 1f / 20f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4 + MathHelper.PiOver2;

            Projectile.velocity = Vector2.Zero;
            TranscendenceUtils.NoExpertProjDamage(Projectile);

            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Projectile.Center + Vector2.One.RotatedBy(rot) * 2500, Projectile.ai[0] * 0.5f * Projectile.scale, ref reference))
                return true;
            else return false;
        }
        public override bool? CanDamage() => Projectile.ai[2] > 80;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            DrawDivineLaser(Projectile, Projectile.ai[2] < 60, (int)Projectile.ai[0], Center, rot, 2500f);

            if (Projectile.ai[0] < 1300f)
                return false;

            return false;
        }
        public static void DrawDivineLaser(Projectile projectile, bool telegraph, int width, Vector2 Center, float rot, float height)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[projectile.type] = 4000;
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine3").Value;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * MathHelper.Lerp(100f, 45f, width / 400f);
            Vector2 posB = Center + Vector2.One.RotatedBy(rot) * 60f;

            Vector2 pos2 = projectile.Center + Vector2.One.RotatedBy(rot) * height;
            Vector2 pos2B = projectile.Center + Vector2.One.RotatedBy(rot) * 4250f;
            int timer = projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;

            projectile.localAI[2] += 0.025f;

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            float a = timer < 5 ? MathHelper.Lerp(0f, 1f, timer / 5f) : 1f;

            sb.Draw(sprite, new Rectangle(
                    (int)(posB.X - Main.screenPosition.X), (int)(posB.Y - Main.screenPosition.Y), (int)(width * 0.75), (int)(posB.Distance(pos2B) * 2f)), null,
                    Color.Orange * a * 0.66f, posB.DirectionTo(pos2B).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.Draw(sprite, new Rectangle(
                (int)(posB.X - Main.screenPosition.X), (int)(posB.Y - Main.screenPosition.Y), width / 12, (int)(posB.Distance(pos2B) * 2f)), null,
                Color.Yellow * a, posB.DirectionTo(pos2B).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);


            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize1"].SetValue(new Vector2(shaderImage.Width * 0.25f, shaderImage.Height * 0.5f));

            eff.Parameters["useAlpha"].SetValue(true);
            eff.Parameters["alpha"].SetValue(projectile.scale);
            eff.Parameters["useExtraCol"].SetValue(true);
            eff.Parameters["extraCol"].SetValue(new Vector3(0.825f, 0.5f, 0f));

            eff.Parameters["uTime"].SetValue(projectile.localAI[2] * 0.5f);
            eff.Parameters["yChange"].SetValue(projectile.localAI[2] * 0.75f);

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 16; i++)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(width * projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            
            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)((width / 2) * projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                Color.White * projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}