using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class Moon : ModProjectile
    {
        public float Rot;
        public Vector2 playerPos;
        public Vector2 vel;
        public override void SetDefaults()
        {
            Projectile.width = 489;
            Projectile.height = 490;

            Projectile.hostile = true;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override Color? GetAlpha(Color lightColor) => Color.Transparent;

        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            Projectile.ai[0] = 1;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (490f * 0.5f * Projectile.scale) && Projectile.localAI[1] > 30)
                return true;
            else return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.DrawEntity(Projectile, Color.White, 5 * Projectile.scale, "bloom3",
                Projectile.direction, Projectile.Center, null);

            int cd = Projectile.localAI[0] == 1f ? 40 : 0;

            if (Projectile.localAI[2] > ((140 - cd))) TranscendenceUtils.DrawEntity(Projectile, Color.MidnightBlue, 7 * Projectile.scale, "bloom",
                Projectile.direction, Projectile.Center, null);

            if (Projectile.localAI[2] > (80 - cd) && Projectile.velocity == Vector2.Zero && !(Main.getGoodWorld || Main.zenithWorld))
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                Vector2 pos = Projectile.Center - Main.screenPosition;
                TranscendenceUtils.DrawEntity(Projectile, Color.Lerp(Color.DarkBlue,
                    Color.White, Projectile.localAI[2] / (120f - cd)) * 0.25f, "TranscendenceMod/Miscannellous/Assets/ExpandingTelegraph",
                    Projectile.DirectionTo(playerPos).ToRotation() + MathHelper.PiOver2, new Rectangle((int)pos.X, (int)pos.Y, (int)(490 * Projectile.scale), 6000), null);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            TranscendenceUtils.DrawTrailProj(Projectile, Color.Gray * 0.7f, Projectile.scale, Texture, false, true, 1, Vector2.Zero);

            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture,
                MathHelper.ToRadians(Rot), Projectile.Center, null);

            return false;
        }

        public override void AI()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;

            Projectile.ai[2]--;
            Projectile.localAI[1]++;

            
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;

            //Main.NewText(Projectile.localAI[2]);

            NPC npc = Main.npc[(int)Projectile.ai[1]];

            if (!npc.active)
                Projectile.Kill();
            
            Projectile.localAI[2]++;

            int cd = Projectile.localAI[0] == 1f ? 20 : 0;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player == null || !player.active)
                    return;

                /*if (player.Distance(Projectile.Center) < (520f * 0.5f * Projectile.scale) && Projectile.localAI[1] > 110 && Projectile.localAI[2] > (130 - cd))
                {
                    player.SetImmuneTimeForAllTypes(15);
                    player.velocity = Projectile.velocity;
                }*/

                if (Projectile.localAI[2] > (80 - cd) && Projectile.localAI[2] < (115 - cd))
                {
                    playerPos = player.Center + player.velocity * 40f;
                    vel = Projectile.DirectionTo(playerPos) * (Projectile.localAI[0] == 1f ? 50f : 30f);
                }
                else if (Projectile.localAI[2] > (130 - cd))
                {
                    Rot++;
                    Projectile.velocity = vel;

                    if (Projectile.localAI[2] > ((170 - cd)))
                        Projectile.localAI[2] = 0;
                }
            }
        }
    }
}