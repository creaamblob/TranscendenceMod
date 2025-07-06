using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment.Tools
{
    public class PortalBoxPoj : ModProjectile
    {
        public int dir;
        public float height;
        public float ScanYPos = 0;
        public float ScanYPosSpeed = 1.275f;
        public Vector2 ScanY = new Vector2(0, 8);
        public override string Texture => "TranscendenceMod/Items/PortalBox";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 28;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = int.MaxValue;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
                Projectile.Kill();

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -dir);
            player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer = 10;
            player.direction = dir;
            player.position = player.oldPosition;
            player.velocity = Vector2.Zero;

            ScanYPos += ScanYPosSpeed;
            if (ScanYPos > 55 || ScanYPos < -55)
                ScanYPosSpeed = -ScanYPosSpeed;

            Projectile.Center = player.Center + new Vector2(35 * player.direction, 10);

            if (height < 1 && Projectile.ai[1] > 30)
                height += 0.025f;
            string tip = "";
            switch (Main.rand.Next(7))
            {
                case 0: tip = "Learn to parry with your shields, you might need that skill at some point."; break;
                case 1: tip = $"The Shimmer's horizontal location is at {GenVars.shimmerPosition.X / 9} * (3^2)"; break;
                case 2: tip = "You might find special trinkets if you explore different places."; break;
                case 3: tip = "When fighting bosses, if you are at an incredibly difficult attack, learn what you are doing wrong and try to improve."; break;
                case 4: tip = "Doing my quests every day will keep a late game grind away."; break;
                case 6: tip = "The water below the Cosmic Cathedral will heal you."; break;
            }


            switch (++Projectile.ai[1])
            {
                case 30: SoundEngine.PlaySound(SoundID.Zombie66, Projectile.Center); break;
                case 150: Main.NewText("Scanning: 0% Complete", Color.DeepSkyBlue); break;
                case 280: Main.NewText("Scanning: 35% Complete", Color.DeepSkyBlue); break;
                case 320: Main.NewText("Scanning: 60% Complete" + " | TIP: " + tip, Color.DeepSkyBlue); break;
                case 430: Main.NewText("Scanning: 90% Complete", Color.DeepSkyBlue); break;
                case 530: Main.NewText("Scanning: 100% Complete", Color.DeepSkyBlue); break;
                case 680: Main.NewText("Transmission: 0% Complete", Color.DeepSkyBlue); break;
                case 720: Main.NewText("Transmission: 20% Complete", Color.DeepSkyBlue); break;
                case 880: Main.NewText("Transmission: 45% Complete" + " | TIP: " + tip, Color.DeepSkyBlue); break;
                case 940: Main.NewText("Transmission: 80% Complete", Color.DeepSkyBlue); break;
                case 1060: Main.NewText("Transmission: 90% Complete", Color.DeepSkyBlue); break;
                case 1130: Main.NewText("Transmission: 100% Complete", Color.DeepSkyBlue); break;
                case 1230:
                    {
                        player.Teleport(TranscendenceWorld.spaceNPCPos - new Vector2(player.width, player.height * 2));
                        Projectile.Kill();
                        break;
                    }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            dir = Main.player[Projectile.owner].direction;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, new Rectangle((int)(Projectile.position.X - Main.screenPosition.X),
                (int)(Projectile.position.Y - Main.screenPosition.Y - (int)MathHelper.Lerp(0f, Projectile.height / 2, height)), Projectile.width, (int)MathHelper.Lerp(0f, Projectile.height, height)), lightColor);

            if (Projectile.ai[1] > 120)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue, 1, "TranscendenceMod/Miscannellous/Assets/TelegraphLine", MathHelper.PiOver2, Main.player[Projectile.owner].Center - new Vector2(6, ScanYPos), null);
                TranscendenceUtils.DrawEntity(Projectile, Color.DeepSkyBlue, 1, "TranscendenceMod/Miscannellous/Assets/TelegraphLine", -MathHelper.PiOver2, Main.player[Projectile.owner].Center - new Vector2(-6, ScanYPos), null);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            return false;
        }
    }
}