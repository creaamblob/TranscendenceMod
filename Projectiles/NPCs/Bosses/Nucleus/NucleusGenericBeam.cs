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

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class NucleusGenericBeam : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public float RotSpeed;
        public float Height;

        public Vector3 col;
        public Color telegraphCol;
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
            Projectile.timeLeft = 600;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

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
                    SoundEngine.PlaySound(ModSoundstyles.SeraphBomb);

                if (Height < 3000)
                    Height += 100f;

                if (Projectile.scale < 1f && Projectile.ai[0] < 15)
                    Projectile.scale += 0.1f;
            }

            if (Projectile.ai[0] > 65f)
            {
                Projectile.scale -= 1f / 15f;
            }

            if (Projectile.ai[0] > 80f)
                Projectile.Kill();
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;

            TranscendenceUtils.NoExpertProjDamage(Projectile);

            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Projectile.Center + Vector2.One.RotatedBy(rot) * 2000, 15f * Projectile.scale, ref reference))
                return true;
            else return false;
        }
        public override bool? CanDamage() => Projectile.ai[0] > 20f && Projectile.ai[0] < 75f;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine3_ALT").Value;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * (10f + Height / 2f);
            Vector2 posB = Center + Vector2.One.RotatedBy(rot) * 1510f;

            Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(rot) * Height;
            Vector2 pos2B = Projectile.Center + Vector2.One.RotatedBy(rot) * 3000f;
            int timer = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer;

            if (col == Vector3.Zero)
                col = new Vector3(1f, 1f, 1f);

            if (telegraphCol == Color.Transparent)
                telegraphCol = Color.White;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SerpentCrack").Value;


            Projectile.localAI[2] += 0.025f;

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
             
            for (int i = 0; i < 3; i++)
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.4f * Projectile.scale, "TranscendenceMod/Miscannellous/Assets/GlowBloom", 0, Center, null);

            float a = timer < 15 ? MathHelper.Lerp(0f, 1f, timer / 15f) : 1f;

            sb.Draw(sprite, new Rectangle(
                    (int)(posB.X - Main.screenPosition.X), (int)(posB.Y - Main.screenPosition.Y), 60, (int)(posB.Distance(pos2B) * 2f)), null,
                    telegraphCol * a * 0.66f, posB.DirectionTo(pos2B).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());

            eff.Parameters["useAlpha"].SetValue(true);
            eff.Parameters["alpha"].SetValue(Projectile.scale);

            eff.Parameters["useExtraCol"].SetValue(true);
            eff.Parameters["extraCol"].SetValue(col);

            eff.Parameters["uTime"].SetValue(Projectile.localAI[2] * 0.5f);
            eff.Parameters["yChange"].SetValue(Projectile.localAI[2] * 0.75f);

            Main.instance.GraphicsDevice.Textures[1] = shaderImage;
            eff.Parameters["uImageSize1"].SetValue(new Vector2(shaderImage.Width * 0.375f, shaderImage.Height * 0.5f));

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 12; i++)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(125 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(170 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                telegraphCol * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}