using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class EasternTalismanProj : ModProjectile
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
            Projectile.penetrate = 5;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override bool PreKill(int timeLeft)
        {
            for (int t = 0; t < 6; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, Projectile.ai[1] == 1 ? DustID.GemDiamond : DustID.GemRuby, new Vector2(0, 5).RotatedBy(MathHelper.TwoPi * t / 6f), 0, Color.White, 2f);
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

            if (Projectile.timeLeft > 210)
                Projectile.velocity *= 0.925f;
            else
            {
                if (Projectile.timeLeft > 180 && !(npc != null && npc.active))
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel * 1.75f, 0.025f);
                    return;
                }
            }

            NPC npc2 = Projectile.FindTargetWithinRange(350, true);
            Player player = Main.player[Projectile.owner];

            if (player == null || !player.active || player.dead || npc != null && !npc.active)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.velocity.Length() < 4 && (npc != null && !npc.active || npc == null))
                Projectile.timeLeft -= 4;
            if (Projectile.timeLeft < 45)
                Projectile.scale -= 1f / 45f;

            if (npc != null && npc.active)
            {
                Projectile.Center = npc.Center + startVel;
            }
            else
            {
                if (npc2 != null && npc2.active && npc2.Distance(Projectile.Center) < 500 && startVel == Vector2.Zero)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(npc2.Center + Vector2.One.RotatedBy(Projectile.DirectionTo(npc2.Center).ToRotation() - MathHelper.PiOver4) * 1000) * 15, 0.05f);
                }
            }
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uSaturation"].SetValue(1f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

            eff.Parameters["uOpacity"].SetValue(1f);

            float r = Main.DiscoR / 255f;
            float g = Main.DiscoG / 255f;
            float b = Main.DiscoB / 255f;

            eff.Parameters["uRotation"].SetValue(Projectile.ai[1] == 2 ? r : Projectile.ai[1] == 1 ? 1 : 0.8f);
            eff.Parameters["uTime"].SetValue(Projectile.ai[1] == 2 ? g : Projectile.ai[1] == 1 ? 1 : 0f);
            eff.Parameters["uDirection"].SetValue(Projectile.ai[1] == 2 ? b : Projectile.ai[1] == 1 ? 1 : 0.2f);

            if (Projectile.velocity.Length() > 2 && !(npc != null && npc.active))
                TranscendenceUtils.BetterDrawTrailProj(Projectile, Color.DeepSkyBlue, Projectile.scale, Texture, 0.1f, true, 1f, Vector2.Zero, MathHelper.PiOver2);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return base.PreDraw(ref lightColor);
        }
    }
}