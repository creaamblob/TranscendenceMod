using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent
{
    public class FrostBlast : ModProjectile
    {
        public float Scale = 0.75f;
        public float Rad;
        public float MaxRad;
        public float Alpha = 1;
        public override string Texture => TranscendenceMod.ASSET_PATH + "/InvisSprite";
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 34;

            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 450;
            Projectile.hostile = true;
            Projectile.extraUpdates = 50;

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
            //Blatantly stolen from Seraph's Supernovas, which were blatantly stolen from Nucleus' Mineblasts

            SpriteBatch spriteBatch = Main.spriteBatch;



            //Rectangle #1
            int x = (int)(Projectile.Center.X - (Projectile.localAI[0] / 2f) - Main.screenPosition.X);
            int y = (int)(Projectile.Center.Y - (Projectile.localAI[0] / 2f) - Main.screenPosition.Y);
            Rectangle rec = new Rectangle(x, y, (int)Projectile.localAI[0], (int)Projectile.localAI[0]);

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;



            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uImageSize1"].SetValue(new Vector2(rec.Width, rec.Height));
            eff.Parameters["maxColors"].SetValue(48);


            //Rectangle #2
            Rectangle rec2 = new Rectangle(x - (int)(Projectile.localAI[0] / 2f), y - (int)(Projectile.localAI[0] / 2f), (int)Projectile.localAI[0] * 2, (int)Projectile.localAI[0] * 2);


            //Additive Blending
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            Color col = Color.DeepSkyBlue;
            Color col2 = Color.White;


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