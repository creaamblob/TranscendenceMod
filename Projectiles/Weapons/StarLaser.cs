using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons
{
    public class StarLaser : ModProjectile
    {
        public float rot;
        public float shaderRot;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/BloomLine2";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.extraUpdates = 2;

            Projectile.aiStyle = -1;
            Projectile.scale = 0;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
            Projectile.ownerHitCheck = true;

            Projectile.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.Center = player.Center + Vector2.One.RotatedBy(rot) * 50;

            shaderRot += 0.1f;
            if (Projectile.timeLeft > 10)
                Projectile.scale += 0.4f;

            if (Projectile.timeLeft < 20 && Projectile.scale > 0)
                Projectile.scale -= 0.4f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TranscendenceUtils.ParticleOrchestra(Terraria.GameContent.Drawing.ParticleOrchestraType.Excalibur, target.Center, -1);
        }
        public override void OnSpawn(IEntitySource source)
        {
            rot = (Projectile.velocity.ToRotation() - MathHelper.PiOver4);
            shaderRot = Main.rand.NextFloat(-5f, 5f);
            Projectile.velocity = Vector2.Zero;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center, player.Center + Vector2.One.RotatedBy(rot) * 2500, 15 * Projectile.scale, ref reference))
                return true;
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 pos = Projectile.Center;
            Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(rot) * 1500f;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/OiledShader").Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size());
            eff.Parameters["uTime"].SetValue(shaderRot.ToRotationVector2().X);
            eff.Parameters["yChange"].SetValue(shaderRot.ToRotationVector2().Y);

            eff.Parameters["useAlpha"].SetValue(false);
            eff.Parameters["useExtraCol"].SetValue(false);

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(10 * Projectile.scale), 2000), null,
                    Color.White, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}