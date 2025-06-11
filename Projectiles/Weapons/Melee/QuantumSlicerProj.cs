using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class QuantumSlicerProj : ModProjectile
    {
        public NPC HitNPC;
        public bool HitWhilePowered;
        public int returnTime = 130;
        public bool ReturnBoom = false;
        public Vector2 dirToTarget;

        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/QuantumSlicer";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;

            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.extraUpdates = 1;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation += 0.5f;

            if (Main.mouseRight && Projectile.ai[2] == 0 && HitNPC == null)
                Projectile.ai[2] = 1;

            if (Projectile.ai[2] == 1)
            {
                Projectile.timeLeft += 30;
                Projectile.velocity *= 1.1f;
                Projectile.damage = (int)(Projectile.damage * 4.25f);
                returnTime = 90;
                Projectile.ai[2] = 2;
            }

            if (Projectile.timeLeft < returnTime)
            {
                Projectile.velocity = Projectile.DirectionTo(player.Center) * 45;

                if (Projectile.Distance(player.Center) < 50)
                    Projectile.Kill();
            }

            if (HitNPC != null && HitNPC.Distance(Projectile.Center) < 150 && Projectile.ai[2] == 0)
            {
                Projectile.Center = HitNPC.Center + dirToTarget;
                Projectile.velocity *= 0.2f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, lightColor, Projectile.scale, Texture, false, true, 1, Vector2.Zero);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, Projectile.scale, Texture + "_Glow", false, true, 1f, Vector2.Zero);
            if (Projectile.ai[2] == 0)
            {
                TranscendenceUtils.DrawEntity(Projectile, Color.Gold, Projectile.scale * 1.25f, Texture, Projectile.rotation, Projectile.Center, null);
                return true;
            }
            return base.PreDraw(ref lightColor);
        }
        public override void PostDraw(Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture + "_Glow", Projectile.rotation, Projectile.Center, null);
        }
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.Excalibur, Projectile.Center, Projectile.whoAmI);
            return base.PreKill(timeLeft);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (HitNPC == null) dirToTarget = Projectile.Center - target.Center;
            HitNPC = target;

            if (Projectile.timeLeft > returnTime)
                HitWhilePowered = true;

            if (Projectile.ai[2] != 0)
            {
                if (Projectile.damage > 50)
                    Projectile.damage = (int)(Projectile.damage * 0.9f);

                int dmg = (int)(Projectile.damage * 0.45f);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<QuantumBlast>(), dmg, 4, Main.player[Projectile.owner].whoAmI, 1, 0.5f, 1);

                HitWhilePowered = true;
            }
        }
    }
}