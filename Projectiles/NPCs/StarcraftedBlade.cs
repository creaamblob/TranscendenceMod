using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs
{
    public class StarcraftedBlade : ModProjectile
    {
        Vector2 intialPos;
        int intialDir;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 53;
            Projectile.height = 42;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 80;
            Projectile.minion = true;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Default;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
        }
        public override void AI()
        {
            TranscendenceUtils.AnimateProj(Projectile, 8);
            if (++Projectile.ai[1] < 5)
            {
                intialDir = (int)Math.Round(Projectile.velocity.X / 5);
                intialPos = Projectile.Center;
            }
            else
            {
                Projectile.spriteDirection = intialDir;
                Projectile.Center = intialPos + new Vector2(intialDir == 1 ? 5 : -50, 0);
            }
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            return base.PreDraw(ref lightColor);
        }
        public override bool? CanHitNPC(NPC target) => !target.friendly && Projectile.frame > 3 && Projectile.frame < 6;
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                for (int i = 0; i < 10; i++)
                    Dust.NewDust(Projectile.Center, 24, 24, ModContent.DustType<ExtraTerrestrialDust>(), 1f, 1f, 255, Color.White, 1.4f);
            }
        }
    }
}