using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class SlamShockwave : ModProjectile
    {
        public float Alpha;
        public Player player;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloom";

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;

            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 9000;

            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
        }
        public override void AI()
        {
            Projectile.ai[0] += 175f;
            if (Projectile.ai[0] > 5250)
                Alpha -= 1f / 120f;

            if (Alpha < 0.02f)
                Projectile.Kill();

            if (Projectile.ai[2] < 1250)
                Projectile.ai[2] += 40f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            int x = (int)(Projectile.Center.X - (Projectile.ai[0] / 4f));
            int y = (int)(Projectile.Center.Y - (Projectile.ai[2] * 0.35f));

            Rectangle rec = new Rectangle(x, y, (int)(Projectile.ai[0] * 0.5f), (int)(Projectile.ai[2] * 0.7f));

            if (targetHitbox.Intersects(rec) && Alpha > 0.75f)
                return true;
            else return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Alpha = 1f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;

            SpriteBatch spriteBatch = Main.spriteBatch;

            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uImageSize1"].SetValue(new Vector2((int)Projectile.ai[0], (int)Projectile.ai[2]));
            eff.Parameters["maxColors"].SetValue(64);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, eff);

            //The Sprite
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            //Coordinates
            int x = (int)(Projectile.Center.X - (Projectile.ai[0] / 2f)) - (int)Main.screenPosition.X;
            int y = (int)(Projectile.Center.Y - (Projectile.ai[2] / 2f)) - (int)Main.screenPosition.Y;

            //Destination Rectangle
            Rectangle rec = new Rectangle(x, y, (int)Projectile.ai[0], (int)Projectile.ai[2]);

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + TranscendenceWorld.UniversalRotation) * 12f;
                int x2 = (int)pos.X;
                int y2 = (int)pos.Y;

                //Destination Rectangle
                Rectangle rec2 = new Rectangle(x + x2, y + y2, (int)Projectile.ai[0], (int)Projectile.ai[2]);

                spriteBatch.Draw(sprite, rec2, null, Color.DeepSkyBlue * 0.75f * Alpha);
            }

            for (int j = 0; j < 4; j++)
                spriteBatch.Draw(sprite, rec, null, Color.White * Alpha);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            return false;
        }
    }
}