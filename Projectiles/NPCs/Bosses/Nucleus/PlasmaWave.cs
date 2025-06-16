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
    public class PlasmaWave : ModProjectile
    {
        public float Alpha;
        public Player player;
        public float Timer;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloom";

        public override void SetDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.tileCollide = false;

            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;

            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
        }
        public override void AI()
        {
            if (Timer > 300)
                Alpha -= 1f / 120f;
            else Alpha += 1f / 45f;

            if (Alpha < 0.02f)
                Projectile.Kill();

            bool expand = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer > 15;
            if (expand)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel * 15f, 0.1f);

            if (Projectile.ai[0] < 1375)
                Projectile.ai[0] += expand ? 22.5f : 7.5f;
            else
                Timer += 1f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;

            Vector2 pos1 = Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4) * (Projectile.ai[0] / 12f);
            Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4) * (Projectile.ai[0] / -12f);

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), pos1, pos2, ((4f + Projectile.ai[0]) / 16f), ref reference))
                return true;
            else return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            Projectile.velocity *= 0.1f;

            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Alpha = 0f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uImageSize1"].SetValue(new Vector2((int)Projectile.ai[0], (int)(Projectile.ai[0] / 12.5f)));
            eff.Parameters["maxColors"].SetValue(48);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, eff);

            //The Sprite
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            //Coordinates
            int x = (int)(Projectile.Center.X) - (int)Main.screenPosition.X;
            int y = (int)(Projectile.Center.Y) - (int)Main.screenPosition.Y;

            //Destination Rectangle
            Rectangle rec = new Rectangle(x, y, (int)Projectile.ai[0], (int)(Projectile.ai[0] / 12.5f));

            float rot = Projectile.velocity.ToRotation();

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + TranscendenceWorld.UniversalRotation) * (Projectile.ai[0] / 80f);
                int x2 = (int)pos.X;
                int y2 = (int)pos.Y;

                //Destination Rectangle
                Rectangle rec2 = new Rectangle(x + x2, y + y2, (int)Projectile.ai[0], (int)(Projectile.ai[0] / 12.5f));

                spriteBatch.Draw(sprite, rec2, null, Color.DeepSkyBlue * Alpha, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0f);
            }

            for (int j = 0; j < 4; j++)
                spriteBatch.Draw(sprite, rec, null, Color.White * Alpha, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0f);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            return false;
        }
    }
}