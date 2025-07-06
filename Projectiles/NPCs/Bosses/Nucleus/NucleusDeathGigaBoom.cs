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
    public class NucleusDeathGigaBoom : ModProjectile
    {
        public float Alpha;
        public Player player;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloom";

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;

            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;

            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            if (Alpha == 1f)
                Projectile.ai[0] += 50f;

            if (Projectile.ai[0] > (3000 + (Projectile.extraUpdates * 750)))
                Alpha -= 1f / 45f;

            if (Alpha < 0.02f)
                Projectile.Kill();
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Alpha = 1f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;


            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uImageSize1"].SetValue(new Vector2((int)Projectile.ai[0], (int)Projectile.ai[0]));
            eff.Parameters["maxColors"].SetValue(128);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, eff);

            //The Sprite
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            //Coordinates
            int x = (int)(Projectile.Center.X - (Projectile.ai[0] / 2f)) - (int)Main.screenPosition.X;
            int y = (int)(Projectile.Center.Y - (Projectile.ai[0] / 2f)) - (int)Main.screenPosition.Y;

            //Destination Rectangle
            Rectangle rec = new Rectangle(x, y, (int)Projectile.ai[0], (int)Projectile.ai[0]);

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + TranscendenceWorld.UniversalRotation) * 32f;
                int x2 = (int)pos.X;
                int y2 = (int)pos.Y;

                //Destination Rectangle
                Rectangle rec2 = new Rectangle(x + x2, y + y2, (int)Projectile.ai[0], (int)Projectile.ai[0]);

                spriteBatch.Draw(sprite, rec2, null, Color.Red * Alpha);
            }

            for (int j = 0; j < 4; j++)
                spriteBatch.Draw(sprite, rec, null, Color.Yellow * Alpha);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            return false;
        }
    }
}