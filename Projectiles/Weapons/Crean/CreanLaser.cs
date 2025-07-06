using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Crean
{
    public class CreanLaser : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/Trail2";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 80;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 1200;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            Asset<Texture2D> sprite2 = TextureAssets.Extra[194];

            MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
            miscShaderData.UseImage1(sprite2);
            miscShaderData.UseSaturation(1f);
            miscShaderData.UseOpacity(5f);
            miscShaderData.Apply();

            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2);
            _vertexStrip.DrawTrail();

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();


            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        private static VertexStrip _vertexStrip = new VertexStrip();
        private Color StripColors(float progressOnStrip) =>
            progressOnStrip < 0.05f ? Color.Transparent :
            Projectile.ai[1] != 0f ? Main.hslToRgb(1f - progressOnStrip, 1f, 0.5f) :
            progressOnStrip < 0.33f ? Color.Lerp(Color.Transparent, Color.White, progressOnStrip) :
            progressOnStrip < 0.66f ? Color.Lerp(Color.White * 0.33f, Color.DeepSkyBlue, (progressOnStrip - 0.33f) * 3f) :
            Color.Lerp(Color.DeepSkyBlue, Color.Blue, (progressOnStrip - 0.66f) * 3f);
        private float StripWidth(float progressOnStrip) => 24f;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (targetHitbox.Distance(Projectile.oldPos[i]) < 12)
                    return true;
            }
            return false;
        }
        public List<NPC> HitNPCs = new List<NPC>();
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (!HitNPCs.Contains(target) && Projectile.localAI[1] == 0)
            {
                if (Projectile.ai[1] != 0f)
                {
                    //for (int i = 0; i < 32; i++)
                      //Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<ArenaDust>(), new Vector2(0, 7.5f + (float)Math.Sin(i * 1.5f) * 1.5f).RotatedBy(MathHelper.TwoPi * i / 32f), 0, Main.hslToRgb(i / 16f, 1f, 0.5f), 2f);
                }
                HitNPCs.Add(target);
                Projectile.localAI[1] = 45;
            }
        }
        public override void AI()
        {
            Projectile.spriteDirection = (int)Projectile.velocity.ToRotation();
            Vector2 vel = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel;

            if (Projectile.localAI[1] > 0)
                Projectile.localAI[1]--;

            //Smaller = Bigger curve
            float multiplier = 0.1f;
            Projectile.velocity = vel.RotatedBy(Math.Sin(++Projectile.ai[0] * Projectile.ai[2] * multiplier));
        }
    }
}