using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class PillarOfCreation : ModProjectile
    {
        Rectangle rec;
        public float Opacity = 1;
        public float Rotation;
        public float Timer;
        Color colour;
        public static float CosmosColorFadeTimer;
        public static float CosmosColorFade = 0.01f;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 450;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<SpaceDebuff>(), 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Rotation += 0.05f;

            rec = new Rectangle((int)Projectile.Center.X - 20,
                (int)((int)Projectile.Center.Y - ((64 + Timer * 5) * ((int)Projectile.ai[0] + 1))),
                52, (int)((85 + Timer * 5) * ((int)Projectile.ai[0] + 1)));

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < ((5 + Timer) * (Projectile.ai[0] + 0.75f)); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Projectile.localAI[1] += 0.1f;
                    float pi = MathHelper.TwoPi * j / 4;
                    float rot = pi += MathHelper.ToRadians(Projectile.localAI[1]) / 10;

                    Vector2 pos = Vector2.One.RotatedBy(rot) * 10;

                    float scale = i > 45 ? MathHelper.Lerp(1f - j, 0.25f, (i - 45) / 45f) : MathHelper.Lerp(0.25f, 1f - j, i / 30f);
                    TranscendenceUtils.DrawEntity(Projectile, new Color(55 + (j * 10) + (i * 5), 5 + j + (i * 3), 15 + (j * 15)) * 0.5f * Opacity, scale, $"Terraria/Images/Projectile_657", Rotation,
                        new Vector2(Projectile.Center.X, Projectile.Center.Y - (i * 8.5f)) - pos, null);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            //spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)(rec.X - Main.screenPosition.X), (int)(rec.Y - Main.screenPosition.Y), rec.Width, rec.Height), Color.Red * 0.5f);

            return base.PreDraw(ref lightColor);
        }
        public override void OnSpawn(IEntitySource source) => TranscendenceUtils.NoExpertProjDamage(Projectile);
        public override void AI()
        {
            

            if (Timer < 30)
                Timer += 0.5f;

            if (Projectile.ai[2] != 0)
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Pi / (200 * Projectile.ai[2])) * 1.05f;

            Dust.NewDust(Projectile.Center - new Vector2(0, Main.rand.Next(0, rec.Height)), 16, -rec.Height,
                ModContent.DustType<VoidDust>(), 0, 0, 0, default, 2);

            Dust.NewDust(Projectile.Center - new Vector2(0, Main.rand.Next(0, rec.Height)), 16, -rec.Height,
                DustID.YellowStarDust, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1.25f);

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj == null || proj == Projectile)
                    return;

                if (proj.Hitbox.Intersects(rec) && proj.type == Projectile.type)
                {
                    SoundEngine.PlaySound(SoundID.AbigailUpgrade with
                    {
                        MaxInstances = 0,
                        Volume = 1.25f
                    }, Projectile.Center);

                    for (int e = 0; e < 6; e++)
                    {
                        Dust.NewDust(Projectile.Center - new Vector2(0, Main.rand.Next(0, 188)), 48, -148,
                            ModContent.DustType<StarDust>(), 0, 0, 0, Color.DarkRed, 2.75f);
                        //TranscendenceUtils.ProjectileRing(Projectile, 8, Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(0, 100) + Main.rand.NextVector2Square(-50, 50), ModContent.ProjectileType<StarMist>(), (int)(Projectile.damage * 1.75f), 3, 0.25f, 0, 0, 1, -1, 0);
                    }
                }
            }

            Opacity -= 0.0025f;
            if (Projectile.ai[0] == 0)
                Projectile.ai[0] = 1;

            /*Vector2 targetVelocity = Projectile.DirectionTo(npc.Center + Vector2.One.RotatedByRandom(360) * Main.rand.Next(20, 130)) * 11;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.15f);*/
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Intersects(rec))
                return true;
            else
                return false;
        }
    }
}