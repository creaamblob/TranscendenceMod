using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons
{
    public class ExoticRayBowShot : ModProjectile
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
            Projectile.width = 6;
            Projectile.height = 6;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            startVel = Projectile.Center - target.Center;
            npc = target;
            Projectile.velocity = Projectile.DirectionTo(npc.Center) * 0.01f;

            Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center + Vector2.One.RotatedByRandom(360) * 200,
                Vector2.Zero, ModContent.ProjectileType<ExoticRayBowTrailingShot>(), Projectile.damage / 2, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
        }
        public override void OnSpawn(IEntitySource source) => SoundEngine.PlaySound(SoundID.Item4);
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item4);
            for (int t = 0; t < 20; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Rainbow>(), Main.rand.NextVector2CircularEdge(1f, 1f) * 5, 0, Main.DiscoColor, 1.75f);
                dust.noGravity = true;

            }
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Player player = Main.player[Projectile.owner];
            if (npc != null)
            {
                Projectile.Center = npc.Center + startVel;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X * 1.25f;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y * 1.25f;
            SoundEngine.PlaySound(SoundID.Item9);
            return false;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            //TranscendenceUtils.DrawEntityOutlines(Projectile, Main.DiscoColor, 1, "TranscendenceMod/Miscannellous/Assets/ExoticRayBowShotThick",
            //Projectile.rotation, Projectile.Center, null);
            if (npc == null)
            {
                TranscendenceUtils.DrawTrailProj(Projectile, Main.DiscoColor, 1, $"{Texture}", true, true, 2, Vector2.Zero);
            }
            TranscendenceUtils.DrawEntityOutlines(Projectile, Main.DiscoColor, 1, $"{Texture}", Projectile.rotation, Projectile.Center, null, 2);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, 1, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}