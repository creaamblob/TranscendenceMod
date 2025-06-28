using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons
{
    public class GasolineProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Default;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity= true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.timeLeft = 300;
        }
        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
                Dust.NewDust(Projectile.Center - new Vector2(2), 4, 4, ModContent.DustType<Oil>(), 0, 2f, 0, Color.White, 1f);
            return base.PreKill(timeLeft);
        }
        public override bool? CanHitNPC(NPC target) => true;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<GasolineBuff>(), 240);
        public override void AI()
        {
            if (Projectile.velocity.Y < 7.5f)
                Projectile.velocity.Y += 0.125f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.BetterDrawTrailProj(Projectile, Color.Black,2f, "TranscendenceMod/Miscannellous/Assets/Circle", 0.25f, false, 2f, Vector2.Zero, 0);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return base.PreDraw(ref lightColor);
        }
    }
}