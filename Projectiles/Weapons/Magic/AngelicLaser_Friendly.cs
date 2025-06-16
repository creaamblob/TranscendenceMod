using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class AngelicLaser_Friendly : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public float randRot;
        public int Lenght;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/WaterBeam";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
            Projectile.scale = 0f;
            Projectile.extraUpdates = 1;
 
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.scale = 0.2f;

            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[2]++;

            if (Lenght < 2000 && Projectile.timeLeft < 60)
                Lenght += 100;

            if (player == null || player.dead || !player.active || player.HeldItem.type != ModContent.ItemType<HolyScroll>())
                Projectile.Kill();

            Center = player.Center + new Vector2(25 * player.direction, -5);
            Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 10;

            if (Projectile.ai[2] == 90)
            {
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                    new Vector2(Main.rand.NextFloatDirection()), 15, 5, 5, -1, null));
                SoundEngine.PlaySound(SoundID.Item67 with { MaxInstances = 0, Volume = 0.1f });
            }

            if (Projectile.scale < 1f && Projectile.ai[2] > 60 && Projectile.timeLeft > 30)
                Projectile.scale += 0.05f;
            if (Projectile.timeLeft < 30)
                Projectile.scale -= 1 / 30f;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            float mult = MathHelper.Lerp(0.025f, 1.25f, target.Distance(Center) / 500f);
            modifiers.FinalDamage *= mult;
        }
        public override void OnSpawn(IEntitySource source)
        {
            randRot = Main.rand.NextFloat(MathHelper.PiOver2);
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;
            
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Projectile.Center + Vector2.One.RotatedBy(rot) * 1250, 16, ref reference))
                return true;
            else return false;
        }
        public override bool? CanDamage() => Projectile.timeLeft < 50;
        public override bool PreDraw(ref Color lightColor) => false;
        public override void PostDraw(Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 700;
            Color col = Main.hslToRgb(Projectile.localAI[1], 1f, 0.5f); //Color.Red;

            if (Projectile.ai[2] < 60)
            {
                sb.Draw(TextureAssets.BlackTile.Value, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), 2, 2000), null,
                    Color.Gold * 0.25f, Projectile.DirectionTo(pos).ToRotation() + MathHelper.PiOver2, TextureAssets.BlackTile.Value.Size() * 0.5f, SpriteEffects.None, 0);
            }
            else
            {
                Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

                sb.End();
                sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(55 * Projectile.scale), 2000), null,
                    Color.Gold * 0.5f, Projectile.DirectionTo(pos).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(10 * Projectile.scale), 2000), null,
                    Color.White, Projectile.DirectionTo(pos).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                sb.End();
                sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            }
        }
    }
}