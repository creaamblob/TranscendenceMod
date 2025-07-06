using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Crean
{
    public class CreanStargazerLaser : ModProjectile
    {

        public int time = 30;
        public float rot;
        public Vector2 Center;
        public Projectile ChosenProjectile;
        public Player player;
        public Vector2 Pos;
        public float rotSpeed;
        public float lenght;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
 
            Projectile.tileCollide = false;
            Projectile.scale = 0.25f;
            Projectile.timeLeft = 50;
            Projectile.extraUpdates = 1;

            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override void AI()
        {
            lenght = 1500;

            rot = ChosenProjectile.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4;


            
            if (!ChosenProjectile.active)
                Projectile.Kill();

            if (Projectile.timeLeft < (time - 5) && ChosenProjectile != null && ChosenProjectile.active)
            {
                Center = ChosenProjectile.Center;
                Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 100;
            }

            if (Projectile.timeLeft == (time - 25))
            {
                SoundEngine.PlaySound(SoundID.Item67 with { MaxInstances = 0 });
                Projectile.scale = Projectile.ai[0];
            }

            if (Projectile.timeLeft < (time - 15) && Projectile.timeLeft > 10 && Projectile.scale < 1f)
                Projectile.scale += 1f / 15f;

            if (Projectile.timeLeft < 5 && Projectile.scale > 0)
                Projectile.scale -= 1f / 5f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;

            ChosenProjectile = Main.projectile[(int)Projectile.ai[1]];
            Projectile.timeLeft = time;

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            Projectile.damage = (int)(Projectile.damage * 0.95f);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), ChosenProjectile.Center, ChosenProjectile.Center + Vector2.One.RotatedBy(rot) * lenght, 16 * Projectile.scale, ref reference))
                return Projectile.timeLeft < (time - 15);

            else return false;
        }
        public override bool? CanDamage() => Projectile.timeLeft < (time - 15);
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            SpriteBatch sb = Main.spriteBatch;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 45;
            Vector2 pos2 = Center + Vector2.One.RotatedBy(rot) * 12;

            if (Projectile.timeLeft > (time - 10))
                return;

            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>($"{Texture}2").Value;
            Texture2D bloom = ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/BloomLine2").Value;

            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            sb.Draw(bloom, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(128f * Projectile.scale), 2000), null,
                new Color(0f, 0.275f, 0.475f) * 2f, Projectile.DirectionTo(Center).ToRotation() - MathHelper.PiOver2, bloom.Size() * 0.5f, SpriteEffects.None, 0);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);

            for (float f = 0; f < 40; f++)
            {
                Vector2 pos3 = Vector2.Lerp(pos, pos + Vector2.One.RotatedBy(rot) * 96 * 0.7f, f);

                sb.Draw(sprite2, new Rectangle(
                (int)(pos3.X - Main.screenPosition.X), (int)(pos3.Y - Main.screenPosition.Y), (int)(32f * Projectile.scale), 96), null,
                Color.White, Projectile.DirectionTo(Center).ToRotation() + MathHelper.PiOver2, sprite2.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.Draw(sprite, new Rectangle(
                (int)(pos2.X - Main.screenPosition.X), (int)(pos2.Y - Main.screenPosition.Y), (int)(32f * Projectile.scale), 14), null,
                Color.White, Projectile.DirectionTo(Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
        }
    }
}