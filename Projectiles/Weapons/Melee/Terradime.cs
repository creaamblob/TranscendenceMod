using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class Terradime : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 3;

            Projectile.light = 0.25f;
            Projectile.timeLeft = 120;

            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            float rand = Main.rand.NextFloat(-0.275f, Main.rand.NextBool(6) ? 0.75f : 0.275f);
            Projectile.scale += rand;
            Projectile.timeLeft = (int)(Projectile.timeLeft * (rand * 1.25f));
            Projectile.rotation = Main.rand.NextFloatDirection();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 3, 3, DustID.TerraBlade, 0f, 0f, 0, default, 1f);
            Main.dust[dust].noGravity = true;
            Projectile.rotation += 0.1f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.TerraBlade, Projectile.Center, Main.player[Projectile.owner].whoAmI);

            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Midas, 180);

            if (damageDone >= target.life && !target.SpawnedFromStatue && !target.TypeName.Contains("Dummy") && !target.friendly)
                Item.NewItem(Projectile.GetSource_OnHit(target), target.getRect(), ItemID.CopperCoin, Main.rand.Next(1, 14));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, $"{Texture}", false, true, 1, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 8, 8, DustID.TerraBlade, 0f, 0f, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(4f, 4f);
            }
            return true;
        }
    }
}