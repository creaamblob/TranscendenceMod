using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class NucleusLaserBoom : ModProjectile
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

            Projectile.hostile = true;
            Projectile.extraUpdates = 2;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            if (Alpha == 1f)
                Projectile.ai[0] += 25f;

            if (Projectile.ai[0] > 375)
                Alpha -= 1f / 45f;

            if (Alpha < 0.02f)
                Projectile.Kill();
            
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (Projectile.ai[0] * 0.33f))
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
            SpriteBatch spriteBatch = Main.spriteBatch;


            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uImageSize1"].SetValue(new Vector2((int)Projectile.ai[0], (int)Projectile.ai[0]));
            eff.Parameters["maxColors"].SetValue(48);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, eff);

            //The Sprite
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            //Coordinates
            int x = (int)(Projectile.Center.X - (Projectile.ai[0] / 2f)) - (int)Main.screenPosition.X;
            int y = (int)(Projectile.Center.Y - (Projectile.ai[0] / 2f)) - (int)Main.screenPosition.Y;

            //Destination Rectangle
            Rectangle rec = new Rectangle(x, y, (int)Projectile.ai[0], (int)Projectile.ai[0]);
            Rectangle rec2 = new Rectangle(x - (int)(Projectile.ai[0] / 2f), y - (int)(Projectile.ai[0] / 2f), (int)Projectile.ai[0] * 2, (int)Projectile.ai[0] * 2);

            spriteBatch.Draw(sprite, rec2, null, Color.Red * Alpha);

            for (int i = 0; i < 3; i++)
                spriteBatch.Draw(sprite, rec, null, Color.White * Alpha);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            return false;
        }
    }
}