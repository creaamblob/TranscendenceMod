using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class NucleusHeart : ModProjectile
    {
        public Vector2 pos;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 42;
            Projectile.tileCollide = false;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 450;
            Projectile.hostile = true;

            Projectile.rotation = MathHelper.Pi;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.localAI[1] += 2f;
            if (Projectile.timeLeft < 15)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 15f);

            if (Projectile.localAI[2] == 1f)
            {
                if (Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer < 75)
                    return;

                if (Projectile.timeLeft < 120)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel * -1.25f, 1f / 45f);
                }
                else Projectile.velocity *= 0.975f;
            }
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public Color col()
        {
            Color col2 = Color.DeepSkyBlue;
            if (Projectile.ai[0] % 2 == 0)
                col2 = Color.Red;

            return col2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, col() * MathHelper.Lerp(0.5f, 0f, Projectile.localAI[1] / 120f), Projectile.scale, TranscendenceMod.ASSET_PATH + "/GlowBloom", Projectile.rotation, Projectile.Center + Projectile.velocity * Projectile.localAI[1], null);

            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (npc != null && npc.active && npc.ModNPC is ProjectNucleus boss)
                TranscendenceUtils.DrawEntity(Projectile, col() * 0.375f, Projectile.scale * 1.5f + (boss.HeartSize * 3f), Texture, Projectile.rotation, Projectile.Center, null);

            for (int i = 0; i < 4; i++)
                TranscendenceUtils.DrawEntity(Projectile, Color.Lerp(col(), Color.White, i / 2f), MathHelper.Lerp(Projectile.scale, Projectile.scale * 0.4f, i / 4f), Texture, Projectile.rotation, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            //ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, Projectile.ai[2].ToString(), Projectile.Bottom - Main.screenPosition, Color.White, 0f, Vector2.Zero, Vector2.One);

            return false;
        }
    }
}