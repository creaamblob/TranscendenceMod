using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class CloudProj : ModProjectile
    {
        Player player;
        Vector2 position = Vector2.Zero;
        Vector2 position2 = Vector2.Zero;
        public float TelegraphFade = 0;
        public ref float iHateGeomertry => ref Projectile.localAI[1];
        public override void SetDefaults()
        {
            Projectile.width = 106;
            Projectile.height = 56;

            Projectile.aiStyle = -1;
            Projectile.scale = 4;

            Projectile.extraUpdates = 2;
            Projectile.hostile = true;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color col = Color.Lerp(Color.White, Color.DarkSlateBlue, Projectile.ai[2] / 180f);
            /*if (Projectile.ai[2] > 160 && Projectile.ai[0] != 1 && player != null && position != Vector2.Zero && Main.npc[(int)Projectile.ai[1]] != null)
            {

                if (Projectile.Distance(position) < 5000 && Main.npc[(int)Projectile.ai[1]].ModNPC is WindDragon boss)
                {
                    float Lenght = 2;

                    sb.End();
                    sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                    sb.Draw(sprite, new Rectangle(
                        (int)(Projectile.Center.X - Main.screenPosition.X),
                        (int)(Projectile.Center.Y - Main.screenPosition.Y),
                        25,
                        (int)(Projectile.Distance(position) * Lenght)), null,
                        col, Projectile.DirectionTo(position).ToRotation() + MathHelper.PiOver2,
                        sprite.Size() * 0.5f,
                        SpriteEffects.None,
                        0);

                    sb.Draw(sprite, new Rectangle(
                         (int)(Projectile.Center.X - Main.screenPosition.X),
                         (int)(Projectile.Center.Y - Main.screenPosition.Y),
                         5,
                         (int)(Projectile.Distance(position) * Lenght)), null,
                         Color.White, Projectile.DirectionTo(position).ToRotation() + MathHelper.PiOver2,
                         sprite.Size() * 0.5f,
                         SpriteEffects.None,
                         0);

                    sb.End();
                    sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }*/
            if (Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer < 90)
            {
                SpriteBatch sb = Main.spriteBatch;
                sb.End();
                sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Gradient").Value;
                sb.Draw(sprite, new Rectangle(
                    (int)(Projectile.Center.X - Main.screenPosition.X) + 275,
                    0,
                    Main.screenHeight * 2,
                    200), null,
                    Color.DeepSkyBlue * TelegraphFade, -MathHelper.PiOver2,
                    sprite.Size() * 0.5f,
                    SpriteEffects.None,
                    0);

                sb.Draw(sprite, new Rectangle(
                    (int)(Projectile.Center.X - Main.screenPosition.X) - 275,
                    0,
                    Main.screenHeight * 2,
                    200), null,
                    Color.DeepSkyBlue * TelegraphFade, MathHelper.PiOver2,
                    sprite.Size() * 0.5f,
                    SpriteEffects.None,
                    0);

                //Some smoothening
                sb.Draw(sprite, new Rectangle(
                    (int)(Projectile.Center.X - Main.screenPosition.X) - 150 ,
                    0,
                    Main.screenHeight * 2,
                    50), null,
                    Color.DeepSkyBlue * TelegraphFade, -MathHelper.PiOver2,
                    sprite.Size() * 0.5f,
                    SpriteEffects.None,
                    0);

                sb.Draw(sprite, new Rectangle(
                    (int)(Projectile.Center.X - Main.screenPosition.X) + 150,
                    0,
                    Main.screenHeight * 2,
                    50), null,
                    Color.DeepSkyBlue * TelegraphFade, MathHelper.PiOver2,
                    sprite.Size() * 0.5f,
                    SpriteEffects.None,
                    0);

                sb.End();
                sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            }

            TranscendenceUtils.DrawEntity(Projectile, Color.White, 1f, TextureAssets.Cloud[18].Value, Projectile.rotation, Projectile.Center, null);
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            position = Main.npc[(int)Projectile.ai[1]].Center;
        }
        public override void AI()
        {
            if (Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer < 90)
            {
                if (Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer < 30 && TelegraphFade < 1)
                {
                    TelegraphFade += 1 / 30f;
                }
                else if (TelegraphFade > 0 && Projectile.GetGlobalProjectile<TranscendenceProjectiles>().Timer < 70)
                    TelegraphFade -= 1 / 20f;
                return;
            }
            /*if (++Projectile.ai[0] > 5)
            {
                if (Projectile.ai[0] < 7) position2 = new Vector2(Projectile.Center.X > position.X ? position.X - 500 : position.X + 500, Projectile.Center.Y);
                Projectile.Center = Projectile.Center.MoveTowards(position2, 9);
                if (Projectile.Distance(position2) < 25)
                {
                    Projectile.Center = position2;
                    Projectile.ai[0] = 0;
                }
            }
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                player = Main.player[i];
                if (player != null && player.Distance(Projectile.Center) < 5000 && Projectile.ai[0] != 1)
                {
                    if (Projectile.ai[2] == 140)
                        position = player.Center;
                    if (++Projectile.ai[2] > 180)
                    {
                        Projectile.velocity = Projectile.DirectionTo(position) * 8;
                        SoundEngine.PlaySound(SoundID.DD2_BookStaffCast);
                        Projectile.ai[0] = 1;
                    }
                    else if (Projectile.localAI[2] != 5)
                    {
                        NPC npc = Main.npc[(int)Projectile.ai[1]];
                        Vector2 pos = npc.Center + iHateGeomertry.ToRotationVector2() * Projectile.ai[2] * 3;
                        Projectile.Center = pos;
                        iHateGeomertry += MathHelper.ToRadians(6) / 12;
                    }
                }
            }
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];
                if (p != null && p.active && p.Hitbox.Intersects(new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height)))
                    p.Center = p.Center.MoveTowards(Projectile.Center, -15);

            }*/

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<Snow>() && proj.Hitbox.Intersects(new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height)))
                    proj.Kill();

            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 12; i++) Dust.NewDust(Projectile.Center, 8, 8, DustID.Cloud);
        }
    }
}