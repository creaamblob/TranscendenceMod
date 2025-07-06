using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles
{
    public class Shockwave : ModProjectile
    {
        public float Rad;
        public float MaxRad;
        public float Alpha = 1;
        public int width;
        public int height;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.timeLeft = Projectile.damage;
            Projectile.extraUpdates = (int)Projectile.knockBack;
            MaxRad = Projectile.timeLeft;
        }
        public override void AI()
        {
            Projectile.position = Projectile.Center;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 8000;

            Projectile.width++; 
            Projectile.height++;

            Projectile.position -= Projectile.Size * 0.5f;
            Alpha = MathHelper.Lerp(0, 2f, (float)Projectile.timeLeft / (float)MaxRad);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            string spritepath = $"Terraria/Images/Misc/Perlin";
            Texture2D sprite = ModContent.Request<Texture2D>(spritepath).Value;
            Color col = new Color(Projectile.ai[0], Projectile.ai[1], Projectile.ai[2]);

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