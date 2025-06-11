using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class CosmicPortal : ModProjectile
    {
        public float Rotation;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 105;

            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.light = 0.75f * Projectile.scale;
            if (Projectile.timeLeft < 40 && Projectile.scale > 0)
                Projectile.scale -= 0.025f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 20, ModContent.DustType<NovaDust>(), 5, Color.White, 2);
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].Distance(Projectile.Center) < 1500 && Projectile.ai[1] == 0)
                {
                    Main.player[Projectile.owner] = Main.player[i];
                    Projectile.ai[1] = Main.player[Projectile.owner].ownedProjectileCounts[Type];
                }
            }
        }
        public override void PostDraw(Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/WaterBeam").Value;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                //projectile.ai[1] == Projectile.ai[1] + 1 && 
                if (projectile.ai[1] == Projectile.ai[1] + 1 && projectile != null && projectile.active && projectile.Center.Distance(Projectile.Center) < 2000 && projectile.type == Type)
                {
                    Vector2 pos = Vector2.Lerp(Projectile.Center, projectile.Center, 0.5f);
                    sb.End();
                    sb.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    sb.Draw(sprite, new Rectangle(
                        (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(25 * Projectile.scale), (int)(Projectile.Distance(projectile.Center))), null,
                        Color.Blue * 0.66f, Projectile.DirectionTo(projectile.Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                   sb.Draw(sprite, new Rectangle(
                        (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(10 * Projectile.scale), (int)(Projectile.Distance(projectile.Center))), null,
                        Color.White * 0.5f, Projectile.DirectionTo(projectile.Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                    sb.End();
                    sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }

            Rotation += 0.05f;

            TranscendenceUtils.DrawEntity(Projectile, Color.Blue * 0.5f, Projectile.scale * 1.5f, "TranscendenceMod/Miscannellous/Assets/Portal", Rotation * 1.5f, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Aqua, Projectile.scale, "TranscendenceMod/Miscannellous/Assets/Portal", -Rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale * 0.6f, "TranscendenceMod/Miscannellous/Assets/Portal", Rotation, Projectile.Center, null);
        }
    }
}