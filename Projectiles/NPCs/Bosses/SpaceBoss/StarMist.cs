using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class StarMist : ModProjectile
    {
        public bool Show;
        public float Alpha;
        public float Dist;
        public NPC npc;
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 900;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public Vector2 pos => Main.npc[(int)Projectile.ai[1]].Center;
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
            Alpha = (Projectile.Center - pos).ToRotation();
            Dist = Projectile.Distance(pos);
        }
        public override bool PreDraw(ref Color lightColor) => false;
        public override void PostDraw(Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SpiritShader2").Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 3f);
            eff.Parameters["uTime"].SetValue(0);
            eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly / 2f);
            eff.Parameters["useExtraCol"].SetValue(true);
            Vector3 col = new Vector3(0f, 0.2f, 0.8f);
            if (Projectile.ai[0] < 0)
            {
                col = Projectile.ai[2] % 2 == 0 ? new Vector3(1f, 0.8f, 0f) : Projectile.ai[2] % 3 == 0 ? new Vector3(1f, 1f, 1f) : new Vector3(0f, 0.4f, 0.8f);
            }
            eff.Parameters["extraCol"].SetValue(col);
            eff.Parameters["useAlpha"].SetValue(true);
            float a = (Projectile.localAI[2] / 45f) * Projectile.Opacity;
            eff.Parameters["alpha"].SetValue(a);

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 2; i++)
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.75f, Texture, 0, Projectile.Center, null);

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.White * 0.5f * a, 0.35f, Texture, 0, Projectile.Center, null);
            for (int i = 0; i < 2; i++)
                TranscendenceUtils.DrawEntity(Projectile, Color.White * a, 0.425f, Texture, 0, Projectile.Center, null);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool CanHitPlayer(Player target) => Projectile.localAI[2] > 85;
        public override void AI()
        {
            if (Projectile.timeLeft < 60)
                Projectile.Opacity -= 1f / 60f;
            else if (Projectile.Opacity < 1f)
                Projectile.Opacity += 1f / 30f;

            float rot = Alpha + TranscendenceWorld.UniversalRotation * 0.75f * Projectile.ai[0];
            Projectile.rotation = rot;
            Projectile.Center = pos + Vector2.One.RotatedBy(rot) * Dist;

            if (Projectile.localAI[2] < 1 || Projectile.ai[0] > 0)
                return;

            Dist -= 5f;
            if (Dist < 50)
            {
                SoundEngine.PlaySound(new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Hurt/CelestialSeraphShield")
                {
                    Pitch = 2f,
                    MaxInstances = 0
                }, Projectile.Center);

                Dist = Main.rand.NextFloat(750f, 1000f);
                Projectile.Opacity = 0f;
            }
            //Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (100 * Projectile.ai[2]));
        }
    }
}