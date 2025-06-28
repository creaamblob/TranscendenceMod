using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class DreamSealProj : ModProjectile
    {
        public Vector2 startVel;
        NPC npc;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.timeLeft = 240;
            Projectile.penetrate = 3;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override bool PreKill(int timeLeft)
        {
            for (int t = 0; t < 8; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<NovaDust>(), new Vector2(0, 5).RotatedBy(MathHelper.TwoPi * t / 6f), 0, Color.White, 2f);
                dust.noGravity = true;
            }
            return base.PreKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            startVel = Projectile.Center - target.Center;
            npc = target;
            Projectile.velocity = Projectile.DirectionTo(npc.Center) * 0.001f;
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            Player player = Main.player[Projectile.owner];

            if (player == null || !player.active || player.dead || npc != null && !npc.active)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.timeLeft < 45)
                Projectile.scale -= 1f / 45f;

            if (npc != null && npc.active)
            {
                Projectile.Center = npc.Center + startVel;
            }
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Color col =
                Main.player[Projectile.owner].statMana < (Main.player[Projectile.owner].statManaMax2 * 0.25f) ? Color.Red
                : Projectile.ai[2] == 1 ? Color.Magenta
                : Color.Purple;

            if (Projectile.velocity.Length() > 2 && !(npc != null && npc.active))
                TranscendenceUtils.BetterDrawTrailProj(Projectile, col, Projectile.scale, Texture, 0f, true, 1f, Vector2.Zero, MathHelper.PiOver2);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return base.PreDraw(ref lightColor);
        }
    }
}