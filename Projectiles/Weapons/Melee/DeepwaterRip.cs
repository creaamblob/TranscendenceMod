using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class DeepwaterSlash : ModProjectile
    {
        public static float Speed;
        public static int DistanceMultiplier = 45;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.noEnchantmentVisuals = true;

            Projectile.timeLeft = 36;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
            Projectile.penetrate = 10;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 17;

            Projectile.extraUpdates = 2;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Speed = 1.025f;
            if (++Projectile.ai[1] > 7)
                Projectile.velocity = Projectile.velocity.RotatedBy(4 * Projectile.ai[2]) * Speed;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Player player = Main.player[Projectile.owner];
            float random = Main.rand.Next(-25, 25);
            float rot = player.DirectionTo(Projectile.Center).ToRotation() + MathHelper.ToRadians(random);
            Vector2 pos = player.Center + Vector2.One.RotatedBy(rot + (MathHelper.PiOver4 / 2f)) * 80f - Main.screenPosition;
            Vector2 pos2 = player.Center + Vector2.One.RotatedBy(rot - (MathHelper.PiOver4 / 2f) - MathHelper.PiOver2) * 80f - Main.screenPosition;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Trail2").Value;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            spriteBatch.Draw(sprite2, new Rectangle((int)(pos.X), (int)(pos.Y), (int)(25 + (Projectile.ai[1] * 3)), 325), null, new Color(1f, 0f, 0f),
                rot + MathHelper.ToRadians(60), sprite2.Size() * 0.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(sprite2, new Rectangle((int)(pos2.X), (int)(pos2.Y), (int)(25 + (Projectile.ai[1] * 3)), 325), null, new Color(1f, 0f, 0f),
                rot - MathHelper.ToRadians(60), sprite2.Size() * 0.5f, SpriteEffects.None, 0);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return base.PreDraw(ref lightColor);
        }
        public override void OnSpawn(IEntitySource source)
        {
        }
        public override void OnHitNPC(NPC victim, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            SoundEngine.PlaySound(SoundID.NPCHit18, victim.Center);
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(victim.position, victim.width, victim.height, ModContent.DustType<BetterBlood>(), victim.DirectionTo(player.Center).X * 10,
                    victim.DirectionTo(player.Center - new Vector2(0, 50)).Y * 20);
            }
        }
    }
}