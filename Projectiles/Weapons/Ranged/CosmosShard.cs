using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged
{
    public class CosmosShard : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/Gores/Projectiles/CosmosShardGore";
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;

            Projectile.timeLeft = 900;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity.X = Projectile.DirectionTo(Main.MouseWorld).X * 17;
            TranscendenceUtils.DustRing(Projectile.Center, 7, ModContent.DustType<ExtraTerrestrialDust>(), 3, Color.White, 1.25f);
        }

        public override void AI()
        {
            if (++Projectile.ai[1] > 15)
                Projectile.velocity *= 1.01f;
        }

        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CosmosShardShockwave>(),
                (int)(Projectile.damage * 0.5f), 7, Main.player[Projectile.owner].whoAmI);
            return base.PreKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 0.8f, $"{Texture}", false, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}