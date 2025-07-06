using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class Meteor : ModProjectile
    {
        public Vector2 pos;
        public readonly int meteor = ModContent.ProjectileType<SpaceRubble>();
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 56;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 5;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 3000;
        }
        public override void OnSpawn(IEntitySource source)
        {
            pos = Projectile.Center;
            Projectile.position.Y -= 750;

            if (Projectile.hostile)
                TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.CircularAoETelegraph(spriteBatch, pos, Color.Red * 0.75f, 1f, 16);


            TranscendenceUtils.DrawEntity(Projectile, Color.White * 0.5f, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
        public override bool? CanDamage() => Projectile.friendly;
        public override void AI()
        {
            Projectile.rotation += 0.2f;
            Projectile.velocity = Projectile.DirectionTo(pos) * 27.5f;

            if (Projectile.Distance(pos) < 25)
            {
                SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 10}, Projectile.Center);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SpaceBossBombBlast>(), 80, 2f, -1, 1f, Projectile.ai[1], 0.625f);
                Main.projectile[p].hostile = Projectile.hostile;
                Main.projectile[p].friendly = Projectile.friendly;

                Projectile.Kill();
            }

        }
    }
}