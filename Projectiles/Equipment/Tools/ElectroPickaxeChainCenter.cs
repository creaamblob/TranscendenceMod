using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Items.Tools;

namespace TranscendenceMod.Projectiles.Equipment.Tools
{
    public class ElectroPickaxeChainCenter : ModProjectile
    {
        public Player player;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 450;

            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }
        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);  
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/WaterBeam").Value;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];

                if (projectile != null && projectile.active && projectile.Center.Distance(Projectile.Center) < 2500 && projectile.type == Type)
                {
                    sb.End();
                    sb.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                    int dist = (int)Projectile.Distance(projectile.Center);
                    float rot = Projectile.DirectionTo(projectile.Center).ToRotation() + MathHelper.PiOver2;
                    Vector2 pos = Vector2.Lerp(Projectile.Center, projectile.Center, 0.5f);

                    float alpha = projectile.scale > 0.9f ? Projectile.scale : projectile.scale;

                    sb.Draw(sprite, new Rectangle(
                        (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(20 * Projectile.scale), dist), null,
                        Color.DeepSkyBlue * 0.75f * alpha, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0);
                    sb.Draw(sprite, new Rectangle(
                        (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(5 * Projectile.scale), dist), null,
                        Color.White * alpha, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                    sb.End();
                    sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }

            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (player == null || !player.active)
                return;

            if (player.HeldItem.type != ModContent.ItemType<ElectroPickaxe>() || player.dead)
                Projectile.Kill();

            if (Projectile.timeLeft < 60 && Projectile.scale > 0)
                Projectile.scale -= 1f / 60f;

            if (++Projectile.ai[2] % 5 == 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    if (projectile != null && projectile.active && projectile.Center.Distance(Projectile.Center) < 2500 && projectile.type == Type)
                    {
                        for (int j = 0; j < 50; j++)
                        {
                            Vector2 pos = Vector2.Lerp(Projectile.Center, projectile.Center, j / 50f);
                            player.PickTile((int)(pos.X / 16), (int)(pos.Y / 16), player.HeldItem.pick);
                        }
                    }
                }
            }
        }
    }
}