using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    // REALLY MESSY CODE UP AHEAD
    public class SpaceBossBomb : ModProjectile
    {
        public float Size = 0.75f;
        public float ColorFade;
        public int BombMaxFuse = 95;
        public bool DontRender;
        public int Timer;
        Color master = new Color(Main.masterColor, Main.masterColor * 0.4f, 0.3f);
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 7;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.tileCollide = false;
            Projectile.timeLeft = BombMaxFuse;

            Projectile.hostile = true;
            Projectile.light = 0.3f;
        }
        public override bool PreKill(int timeLeft)
        {
            if (OnScreen)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<SpaceBossBombBlast>(), Projectile.damage, 8f, -1, 0, Projectile.ai[1], 0.85f);
                SoundEngine.PlaySound(ModSoundstyles.SeraphBomb with { MaxInstances = 0, Volume = 0.2f }, Projectile.Center);
            }
            return base.PreKill(timeLeft);
        }
        public bool OnScreen => Projectile.Center.Between(Main.screenPosition - new Vector2(Main.screenWidth * 1.2f, Main.screenHeight * 1.2f), Main.screenPosition + new Vector2(Main.screenWidth * 1.2f, Main.screenHeight * 1.2f));
        public override void AI()
        {
            if (ColorFade < 1) ColorFade += 0.03f;
            Timer++;

            DontRender = false;
        }
        public override bool CanHitPlayer(Player target) => Timer > 80;
        public override void OnSpawn(IEntitySource source)
        {
            
            ColorFade = 0;

            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override Color? GetAlpha(Color lightColor) => Timer > 60 ? Color.Lerp(Color.Red, Color.Transparent, (float)Math.Sin(Timer / 2)) : Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            if (!OnScreen)
                return false;

            TranscendenceUtils.AnimateProj(Projectile, 8);

            Texture2D sprite3 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;

            if (!DontRender)
            {


                //Request effect
                var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

                eff.Parameters["uImageSize1"].SetValue(new Vector2(sprite3.Width * 2.25f, sprite3.Height * 2.25f));
                eff.Parameters["maxColors"].SetValue(12);



                SpriteBatch spriteBatch = Main.spriteBatch;

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

                Color col = Projectile.localAI[1] == 1 ? new Color(0.3f, 0.15f, 0.5f) : new Color(0.5f, 0.3f, 0.4f);
                col = Color.Lerp(col, Color.Red * 0.5f, Timer / 75);

                Main.EntitySpriteDraw(sprite3, Projectile.Center - Main.screenPosition, null, col * ColorFade,
                    0, sprite3.Size() * 0.5f, 2.25f, SpriteEffects.None);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            }

            return !DontRender;
        }
    }
    public class SpaceBossBombBlast : ModProjectile
    {
        public float Scale = 0.75f;
        int Timer;
        public float Rad;
        public float MaxRad;
        public float Alpha = 1;
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 7;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 34;

            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 450;

            Projectile.noEnchantmentVisuals = true;
            Projectile.hostile = true;
            Projectile.light = 1.25f;
            Projectile.extraUpdates = 25;

            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 3000;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (Projectile.localAI[0] * 0.33f) && Alpha > 0.75f)
                return true;
            else return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            MaxRad = 450 * Projectile.ai[2];
            Projectile.timeLeft = (int)MaxRad;
        }
        public override void AI()
        {
            Projectile.position = Projectile.Center;
            

            //Size
            Projectile.localAI[0]++;

            Alpha = MathHelper.Lerp(0, 2, (float)Projectile.timeLeft / (float)MaxRad);
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            //Blatantly stolen from Project Nucleus' Mineblasts

            SpriteBatch spriteBatch = Main.spriteBatch;



            //Rectangle #1
            int x = (int)(Projectile.Center.X - (Projectile.localAI[0] / 2f) - Main.screenPosition.X);
            int y = (int)(Projectile.Center.Y - (Projectile.localAI[0] / 2f) - Main.screenPosition.Y);
            Rectangle rec = new Rectangle(x, y, (int)Projectile.localAI[0], (int)Projectile.localAI[0]);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;



            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uImageSize1"].SetValue(new Vector2(rec.Width, rec.Height));
            eff.Parameters["maxColors"].SetValue(24);


            //Colorful Plasma Aura
            Rectangle rec2 = new Rectangle(x - (int)(Projectile.localAI[0] / 2f), y - (int)(Projectile.localAI[0] / 2f), (int)Projectile.localAI[0] * 2, (int)Projectile.localAI[0] * 2);


            //Additive Blending
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            Color col = TranscendenceWorld.CosmicPurple;
            Color col2 = Color.White;
            if (Projectile.ai[0] == 1f)
                col = Color.OrangeRed;


            spriteBatch.Draw(sprite, rec2, null, col * Alpha);


            //The Blast
            for (int i = 0; i < 3; i++)
                spriteBatch.Draw(sprite, rec, null, col2 * Alpha);


            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}