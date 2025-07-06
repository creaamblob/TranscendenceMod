using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class OrionsHelixFriendly : ModProjectile
    {
        public int Timer;
        public float Rotation;
        public float DesiredDirection;

        public float GalaxyColorFadeTimer = 520;
        public float GalaxyColorFade = 0.05f;
        public Vector2 vel;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 80;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;

            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;

            Projectile.friendly = true;
            Projectile.tileCollide = false;

            Projectile.localNPCHitCooldown = 33;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ArmorPenetration = 20;

            Projectile.ignoreWater = true;
            Projectile.light = 0.25f;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            

            if (Main.rand.NextBool(7))
            {
                int d = Dust.NewDust(Projectile.Center, 16, 16, DustID.DungeonSpirit, 0, 0, 0, Color.White, 0.75f);
                Main.dust[d].noGravity = true;
            }

            GalaxyColorFadeTimer += GalaxyColorFade;

            if (Projectile.timeLeft > 595)
            {
                vel = Projectile.velocity;
                return;
            }

            if (Projectile.localAI[1] < 5)
            {
                //Smaller = Bigger curve
                float multiplier = 0.3f;
                Projectile.velocity = vel.RotatedBy(Math.Sin(GalaxyColorFadeTimer * Projectile.ai[2] * multiplier));
                //Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / Projectile.ai[2]);
            }/*

            if (++Projectile.localAI[1] > 60)
            {
                Projectile.ai[2] = -Projectile.ai[2] * 0.9f;
                Projectile.localAI[1] = 0;
            }*/
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (targetHitbox.Distance(Projectile.oldPos[i]) < 4 && Projectile.timeLeft < 960)
                    return true;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            Texture2D sprite4 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Trail").Value;
            Vector2 origin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);

            SpriteBatch sb = Main.spriteBatch;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphFontShader").Value;

            Projectile.localAI[2] += 0.0025f;

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size());
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uTime"].SetValue(Projectile.localAI[2] * 0.75f);
            eff.Parameters["yChange"].SetValue(Projectile.localAI[2] * 0.75f);

            eff.Parameters["useAlpha"].SetValue(false);
            eff.Parameters["useExtraCol"].SetValue(false);

            for (int i = 0; i < (((Projectile.oldPos.Length / 2) - 2)); i++)
            {
                Vector2 pos = Projectile.oldPos[i * 2] - origin - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);

                Main.EntitySpriteDraw(sprite4, pos, null, Color.White, Projectile.oldRot[i * 2], sprite4.Size() * 0.5f, Projectile.scale * 1.5f, SpriteEffects.None);
            }

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < (((Projectile.oldPos.Length / 2) - 2)); i++)
            {
                Vector2 pos = Projectile.oldPos[i * 2] - origin - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);

                Main.EntitySpriteDraw(sprite4, pos, null, Color.White, Projectile.oldRot[i * 2], sprite4.Size() * 0.5f, Projectile.scale * 0.75f, SpriteEffects.None);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}