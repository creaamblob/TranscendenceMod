using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class RoarShockwave : ModProjectile
    {
        public float Rad;
        public float MaxRad;
        public float Alpha = 1;
        public int Timer;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.timeLeft = 2500;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            MaxRad = Projectile.timeLeft;
        }
        public override void AI()
        {
            Projectile.position = Projectile.Center;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2500;
            

            if (++Timer > 5 && Projectile.extraUpdates < 100)
            {
                Projectile.extraUpdates += 2;
                Timer = 0;
            }

            Projectile.width++; 
            Projectile.height++;

            Projectile.position -= Projectile.Size * 0.5f;
            Alpha = MathHelper.Lerp(0, 2, (float)Projectile.timeLeft / (float)MaxRad);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center + new Vector2(Projectile.width * 0.25f, Projectile.height * 0.15f)) < (Projectile.width * 0.33f) && Projectile.timeLeft > 800)
                return true;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            string spritepath = "TranscendenceMod/Miscannellous/Assets/Perlin2";
            Texture2D sprite = ModContent.Request<Texture2D>(spritepath).Value;
            Color col = Color.Lerp(Color.White, Color.Gold, (float)Math.Sin(Projectile.width / 64));

            Vector2 scale = new Vector2(1.5f, 1f);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + Projectile.Size * scale * 0.66f;
            Rectangle drawArea = new Rectangle(0, 0, Projectile.width, Projectile.height);

            DrawData drawData = new DrawData(sprite, drawPosition, drawArea, col * Alpha, 0, Projectile.Size, scale, SpriteEffects.None);

            GameShaders.Misc["ForceField"].UseColor(col * Alpha);
            GameShaders.Misc["ForceField"].Apply(drawData);
            drawData.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}