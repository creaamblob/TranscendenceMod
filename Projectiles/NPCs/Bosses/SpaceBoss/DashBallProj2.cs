using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class DashBallProj2 : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG";
        public int Timer;
        public float ChaseSpeed = 15;
        public int Frame;
        Color col;
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            
            if ((Projectile.ai[0] > 0 && Projectile.ai[0] < 3 && Projectile.ai[2] == 1) || Projectile.localAI[1] == 1)
            {
                if (++Projectile.localAI[2] < (Projectile.localAI[1] == 1 ? 40 : 20))
                    Projectile.velocity *= 0.95f;
                else Projectile.velocity = Vector2.Multiply(Projectile.velocity, 1.025f);
            }
            if (Projectile.timeLeft < 90)
            {
                Projectile.ai[2] *= 0.98f;
                Projectile.damage = 0;
            }

            if (Projectile.GetGlobalProjectile<TranscendenceProjectiles>().StupidInt == 1)
            {
                NPC npc = Main.npc[(int)Projectile.ai[1]];
                if (npc == null) return;

                Projectile.velocity = Projectile.DirectionTo(npc.Center) * Projectile.ai[0];

                if (Projectile.Distance(npc.Center) < 175)
                    Projectile.Kill();
            }

            float rot = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.rotation = rot;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (32 * Projectile.ai[2]) && Projectile.active && Projectile != null && Projectile.timeLeft > 90 && Projectile.active)
            {
                return true;
            }
            else return base.Colliding(projHitbox, targetHitbox);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            switch (Projectile.ai[0])
            {
                case 0: col = Color.Magenta; break;
                case 1: col = Color.OrangeRed; break;
                case 2: col = Color.DodgerBlue; break;
                case 3: col = Color.Red; break;
                case 4: col = Color.Blue; break;
                case 5: col = Color.Purple; break;
                case 6: col = Color.MidnightBlue; break;
                case 7: col = Color.Gray; break;
                case 8: col = Color.Gold; break;
                case 9: col = Color.BlueViolet; break;
                case 10: col = Main.DiscoColor; break;
                case 11: col = Color.Blue; break;
            }

            SpriteBatch sb = Main.spriteBatch;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size() * 3f);
            eff.Parameters["uTime"].SetValue(0);
            eff.Parameters["yChange"].SetValue(Main.GlobalTimeWrappedHourly / 2f);
            eff.Parameters["useAlpha"].SetValue(false);
            eff.Parameters["useExtraCol"].SetValue(true);
            eff.Parameters["extraCol"].SetValue(new Vector3(col.R / 255f, col.G / 255f, col.B / 255f));

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 2; i++)
                TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.5f, Texture, 0, Projectile.Center, null);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
    }
}