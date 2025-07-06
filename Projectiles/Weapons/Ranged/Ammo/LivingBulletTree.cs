using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Ranged.Ammo
{
    public class LivingBulletTree : ModProjectile
    {
        public Vector2 startVel;
        NPC npc;
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
            Projectile.tileCollide = false;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            if (Projectile.timeLeft == 299)
                Projectile.velocity *= 0.0001f;

            if (Projectile.timeLeft < 20)
                Projectile.scale -= 0.05f;

            if (npc != null)
            {
                Projectile.Center = npc.Center + startVel;
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            NPC npc2 = Projectile.FindTargetWithinRange(500);

            if (npc2 != null)
            {
                startVel = Projectile.Center - npc2.Center;
                npc = npc2;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            TranscendenceUtils.DustRing(Projectile.Center, 15, DustID.WoodFurniture, 4, Color.White, 1.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, lightColor, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}