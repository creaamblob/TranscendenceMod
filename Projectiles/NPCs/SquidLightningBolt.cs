using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs
{
    public class SquidLightningBolt : ModProjectile
    {
        public override void SetStaticDefaults() => Main.projFrames[Type] = 2;
        public override void SetDefaults()
        {
            Projectile.width = 9;
            Projectile.height = 9;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
            Projectile.timeLeft = 600;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void OnSpawn(IEntitySource source) => TranscendenceUtils.NoExpertProjDamage(Projectile);
        public override bool OnTileCollide(Vector2 oldVelocity) => false;
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            if (++Projectile.ai[2] > 120)
            {
                TranscendenceUtils.ProjectileRing(Projectile, 6, Projectile.GetSource_FromAI(), Projectile.Center,
                   ProjectileID.MartianTurretBolt, (Main.expertMode || Main.masterMode) ? 65 : 90, 0, 2, 0, Projectile.ai[1], 0, -1, Main.rand.NextFloat(MathHelper.TwoPi));
                Projectile.Kill();
            }
        }
        public override void PostDraw(Color lightColor)
        {
            TranscendenceUtils.AnimateProj(Projectile, 3);
            TranscendenceUtils.DrawTrailProj(Projectile, Color.Aqua, 0.4f, "TranscendenceMod/Miscannellous/Assets/Trail", false, false, 1, Vector2.Zero);
        }
    }
}