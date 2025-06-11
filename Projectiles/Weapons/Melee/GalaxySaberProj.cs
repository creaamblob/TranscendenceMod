using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class GalaxySaberProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/GalaxySaber";
        public override string GlowTexture => "TranscendenceMod/Items/Weapons/Melee/GalaxySaber_Glow";
        public bool HomeOn;
        public Vector2 dustDir;
        public override void SetDefaults()
        {
            Projectile.width = 33;
            Projectile.height = 30;

            Projectile.timeLeft = 2400;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.extraUpdates = 1;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.localNPCHitCooldown = Projectile.velocity == Vector2.Zero ? 20 : 0;

            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation += MathHelper.ToRadians(25);
                Dust.NewDustPerfect(Projectile.Center + Vector2.One.RotatedBy(Projectile.rotation - MathHelper.PiOver2) * 20f, ModContent.DustType<ArenaDust>(), Vector2.Zero, 0, Color.Magenta, 1f);
            }
            else
            {
                Projectile.rotation = dustDir.ToRotation() + MathHelper.PiOver4;
                Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<ExtraTerrestrialDust>(), dustDir, 0, Color.White, 1f);
                d.noGravity = true;
            }

            if (HomeOn)
            {
                Projectile.velocity = Projectile.DirectionTo(player.Center) * 45;
                Projectile.tileCollide = false;
            }

            if (Projectile.timeLeft < 300 || Projectile.timeLeft < 2380 && player.altFunctionUse == 2 || Projectile.Distance(player.Center) > 500)
            {
                HomeOn = true;
            }
            if (Projectile.timeLeft < 2380 && Projectile.Distance(player.Center) < 40)
            {
                SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
                Projectile.Kill();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.rotation = oldVelocity.ToRotation();
            dustDir = oldVelocity;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, null);
            return false;
        }
    }
}