using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons
{
    public class Terrarrow : ModProjectile
    {
        public Vector2 startVel;
        public NPC npc;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 15;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 48;
            ProjectileID.Sets.TrailCacheLength[Type] = 45;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TranscendenceUtils.ParticleOrchestra(Terraria.GameContent.Drawing.ParticleOrchestraType.TerraBlade, target.position, -1);

            for (int i = 0; i < 2; i ++)
            {
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center + Vector2.One.RotatedByRandom(360) * Main.rand.NextFloat(0f, 220f),
                    Vector2.Zero, ModContent.ProjectileType<TerraWhirlind>(), Projectile.damage / 2, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
            }
        }
        public override void OnSpawn(IEntitySource source) { }
        public override bool PreKill(int timeLeft)
        {
            for (int t = 0; t < 10; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.TerraBlade, Main.rand.NextVector2CircularEdge(1f, 1f) * 5, 0, Color.White, 1.75f);
                dust.noGravity = true;

            }
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.velocity.Y += 0.05f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
            SoundEngine.PlaySound(SoundID.Item9);
            return false;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            if (npc == null)
            {
                TranscendenceUtils.DrawTrailProj(Projectile, Color.DarkGreen * 0.5f, 1f, "TranscendenceMod/Miscannellous/Assets/Trail", true, true, 2f, new Vector2(-16, 0));
                TranscendenceUtils.DrawTrailProj(Projectile, Color.LimeGreen, 0.75f, "TranscendenceMod/Miscannellous/Assets/Trail", true, true, 1f, new Vector2(-16, 0));
            }
            TranscendenceUtils.DrawEntity(Projectile, Color.White, 1, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}