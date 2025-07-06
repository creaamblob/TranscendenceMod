using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class WaterKunaiProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Magic/WaterKunai";
        public int Timer;
        public float Chase = 15;
        public bool Chasing;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.timeLeft = 300;
            Projectile.penetrate = 5;

            Projectile.width = 6;
            Projectile.height = 6;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Wet, 150);
        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.Wet, 150);
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 15, DustID.DungeonWater, 4, Color.White, 1.5f);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        }
        public override void AI()
        {
            int d = Dust.NewDust(Projectile.Center, 1, 1, DustID.DungeonWater);
            Main.dust[d].noGravity = true;
            Lighting.AddLight(Projectile.Center, 0, 0, 0.8f);

            if ((++Timer == 120 || Main.player[Projectile.owner].altFunctionUse == 2) && !Chasing)
            {
                Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * Chase;
                Chasing = true;
            }
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawTrailProj(Projectile, Color.DarkGray, Projectile.scale, $"{Texture}", false, true, 1.5f, Vector2.Zero);
            return base.PreDraw(ref lightColor);
        }
    }
}