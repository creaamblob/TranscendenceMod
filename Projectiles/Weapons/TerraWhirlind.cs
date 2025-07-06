using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons
{
    public class TerraWhirlind : ModProjectile
    {
        public Vector2 startVel;
        public NPC npc;
        public float StarSize;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = -1;
            Projectile.scale = 0f;

            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
        }
        public override void AI()
        {
            Projectile.rotation += 0.125f;
            Player player = Main.player[Projectile.owner];

            if (Projectile.timeLeft > 90 && Projectile.scale < 0.125f)
                Projectile.scale += 0.025f;
            else
            {
                if (player != null && player.active && !player.dead)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center) * 3.25f, 0.1f);
                    if (Projectile.Distance(player.Center) < 30 && Projectile.ai[1] != 1)
                    {
                        player.statMana += 30;
                        player.ManaEffect(30);
                        Projectile.ai[1] = 1;
                    }
                }
            }

            if (Projectile.timeLeft < 90 && Projectile.timeLeft > 45)
            {
                StarSize += 1f / 30f;
            }

            if (Projectile.timeLeft < 45 || Projectile.ai[1] == 1)
                Projectile.scale -= 0.125f / 45f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (128 * Projectile.scale))
                return true;
            else return false;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.Gold * 0.5f, 40f * Projectile.scale * StarSize, "TranscendenceMod/Miscannellous/Assets/StarEffect", MathHelper.PiOver4, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Lime * 0.75f, 5f * Projectile.scale * StarSize, "TranscendenceMod/Miscannellous/Assets/GlowBloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Lime * 0.5f, 25f * Projectile.scale * StarSize, "TranscendenceMod/Miscannellous/Assets/StarEffect", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, 15f * Projectile.scale * StarSize, "TranscendenceMod/Miscannellous/Assets/StarEffect", 0, Projectile.Center, null);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}