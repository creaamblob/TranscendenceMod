using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class LightningBoss : ModProjectile
    {
        public int Timer;
        Vector2 vel;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 900;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            AIType = ProjectileID.Bullet;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            SoundEngine.PlaySound(SoundID.Thunder);
            vel = Projectile.velocity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}_Body").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>($"{Texture}_Tail").Value;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 origin = new Vector2(sprite.Width * 0.5f, Projectile.height * 0.5f);

            for (int i = 1; i < ((Projectile.oldPos.Length - 1) / 3); i++)
            {
                Vector2 pos = Projectile.oldPos[i * 3] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);

                Main.EntitySpriteDraw(sprite, pos, null, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
            }
            Vector2 pos2 = Projectile.oldPos[39] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);
            Main.EntitySpriteDraw(sprite2, pos2, null, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);

            if (Projectile.localAI[1] < 5)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                TranscendenceUtils.DrawEntity(Projectile, Color.Aqua, 1, "TranscendenceMod/Miscannellous/Assets/BloomLine", vel.ToRotation() + MathHelper.PiOver2, Projectile.Center, null);
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            return true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (targetHitbox.Distance(Projectile.oldPos[i]) < 6 && Projectile.localAI[1] > 5)
                    return true;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.ai[2]--;
            if (++Projectile.localAI[1] > 5)
                Projectile.velocity = vel;
            else if (Projectile.localAI[1] > 1 && Projectile.localAI[1] < 5)
                Projectile.velocity = Vector2.Zero;
            Projectile.spriteDirection = (int)Projectile.velocity.ToRotation();
            /*if (Projectile.velocity == Vector2.Zero && Projectile.timeLeft < 875)
                Projectile.Kill();*/
        }
    }
}