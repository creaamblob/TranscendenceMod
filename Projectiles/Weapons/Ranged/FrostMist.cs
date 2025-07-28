using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class FrostMist : ModProjectile
    {
        public override string Texture => TranscendenceMod.ASSET_PATH + "/Smoke";


        public override void SetStaticDefaults() => Main.projFrames[Type] = 3;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = true;
            Projectile.timeLeft = 30;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.ai[2] += 1f / 30f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            Projectile.frame = Main.rand.NextFromList<int>(0, 200, 400);
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle frame = new Rectangle(0, Projectile.frame, 250, 200);

            sb.Draw(sprite, Projectile.Center - Main.screenPosition, frame, new Color(0f, 0.25f, 0.75f, 0f) * 0.375f * (1f - Projectile.ai[2]), Projectile.rotation, frame.Size() * 0.5f, Projectile.scale * 0.5f, SpriteEffects.None, 0);
            sb.Draw(sprite, Projectile.Center - Main.screenPosition, frame, new Color(0f, 0.5f, 1f, 0f) * 0.375f * (1f - Projectile.ai[2]), Projectile.rotation, frame.Size() * 0.5f, Projectile.scale * 0.25f, SpriteEffects.None, 0);

            return false;
        }
    }
}