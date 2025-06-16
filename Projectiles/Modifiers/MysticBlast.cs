using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Modifiers
{
    public class MysticBlast : ModProjectile
    {
        public float Alpha;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloom";

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;

            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;

            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
        }
        public override void AI()
        {
            if (Alpha == 1f)
                Projectile.ai[0] += 25f;

            if (Projectile.ai[0] > (100 * Projectile.ai[2]))
                Alpha -= 1f / 45f;

            if (Alpha < 0.02f)
                Projectile.Kill();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (Projectile.ai[0] * 0.3f) && Alpha > 0.75f)
                return true;
            else return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[2] = Main.player[Projectile.owner].GetModPlayer<TranscendencePlayer>().MysticCards;

            SoundEngine.PlaySound(ModSoundstyles.SeraphBomb with { MaxInstances = 0 }, Projectile.Center);
            Alpha = 1f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;


            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uImageSize1"].SetValue(new Vector2((int)Projectile.ai[0], (int)Projectile.ai[0]));
            eff.Parameters["maxColors"].SetValue(32);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, eff);

            //The Sprite
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

            //Coordinates
            int x = (int)(Projectile.Center.X - (Projectile.ai[0] / 2f)) - (int)Main.screenPosition.X;
            int y = (int)(Projectile.Center.Y - (Projectile.ai[0] / 2f)) - (int)Main.screenPosition.Y;

            for (int i = 0; i < 4; i++)
            {
                int s = i * 50 ;

                //Destination Rectangle
                Rectangle rec = new Rectangle(x - s, y - s, (int)Projectile.ai[0] + (s * 2), (int)Projectile.ai[0] + (s * 2));

                spriteBatch.Draw(sprite, rec, null, Color.Lerp(Color.Magenta, Color.White, i / 4f) * Alpha);
            }


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            return false;
        }
    }
}