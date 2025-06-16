using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs
{
    public class MosquitoPoison : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Projectiles/Weapons/Melee/MosquitoBiteGas";
        public override void SetStaticDefaults() => Main.projFrames[Type] = 4;
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 28;

            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;

            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Projectile.alpha -= 5;
            if (Projectile.ai[1] != 1) Projectile.velocity *= 0.95f;
            TranscendenceUtils.AnimateProj(Projectile, 5);

            if (Projectile.timeLeft < 30)
                Projectile.scale -= 1f / 30f;
        }
        public override bool CanHitPlayer(Player target) => Projectile.alpha < 150;
        public override void OnSpawn(IEntitySource source) => TranscendenceUtils.NoExpertProjDamage(Projectile);
        public override bool PreKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 10, DustID.Poisoned, Main.rand.Next(-3, 3),
                Color.White, Main.rand.NextFloat(0.3f, 1.4f));
            return base.PreKill(timeLeft);
        }
    }
}