using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs
{
    public class AngelicLaser : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public float randRot;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/WaterBeam";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 3000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
 
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.scale = 0.2f;
            Projectile.extraUpdates = 2;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.ai[2]++;
            Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 10;


           // if (Projectile.ai[2] % 15 == 0)
            //    randRot = Main.rand.NextFloat(MathHelper.ToRadians(360));

           // if (Projectile.ai[2] < 80 && Projectile.timeLeft < 165)
               // rot = MathHelper.Lerp(rot, randRot, 0.01f);

            if (Projectile.ai[2] == 40)
            {
                SoundEngine.PlaySound(SoundID.Zombie104 with { MaxInstances = 0, Volume = 0.1f });
            }

            if (Projectile.scale < 1f && Projectile.ai[2] > 40 && Projectile.timeLeft > 30)
                Projectile.scale += 0.05f;
            if (Projectile.timeLeft < 30)
                Projectile.scale -= 1 / 30f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;
            TranscendenceUtils.NoExpertProjDamage(Projectile);

            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Center + Vector2.One.RotatedBy(rot) * 3750, 10 * Projectile.scale, ref reference))
                return true;
            else return false;
        }
        public override bool? CanDamage() => Projectile.ai[2] > 60;
        public override bool PreDraw(ref Color lightColor) => false;
        public override void PostDraw(Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            Vector2 sPos = Center + Vector2.One.RotatedBy(rot) * (3750 / 2f);
            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 3750;

            if (Projectile.ai[2] < 40)
            {
                sb.Draw(TextureAssets.BlackTile.Value, new Rectangle(
                    (int)(sPos.X - Main.screenPosition.X), (int)(sPos.Y - Main.screenPosition.Y), 2, (int)Center.Distance(pos)), null,
                    Color.Gold, sPos.DirectionTo(pos).ToRotation() + MathHelper.PiOver2, TextureAssets.BlackTile.Value.Size() * 0.5f, SpriteEffects.None, 0);
            }
            else
            {
                Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

                sb.End();
                sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                sb.Draw(sprite, new Rectangle(
                    (int)(sPos.X - Main.screenPosition.X), (int)(sPos.Y - Main.screenPosition.Y), (int)(95 * Projectile.scale), (int)Center.Distance(pos)), null,
                    Color.Gold * 0.75f, sPos.DirectionTo(pos).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                sb.Draw(sprite, new Rectangle(
                    (int)(sPos.X - Main.screenPosition.X), (int)(sPos.Y - Main.screenPosition.Y), (int)(55 * Projectile.scale), (int)Center.Distance(pos)), null,
                    Color.White, sPos.DirectionTo(pos).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                sb.End();
                sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            }
        }
    }
}