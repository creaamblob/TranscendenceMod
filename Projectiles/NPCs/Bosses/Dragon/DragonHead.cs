using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class DragonHead : ModProjectile
    {
        public int Timer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Vector2 velocityspread4 = new Vector2(-8, 0);
            for (int i = 0; i < 8; i++)
            {
                velocityspread4 = velocityspread4.RotatedBy(MathHelper.PiOver2);

                int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocityspread4, ModContent.ProjectileType<CondensedAir>(), Projectile.damage, 2, -1, Projectile.ai[0], Projectile.whoAmI);
                Main.projectile[proj].localAI[2] = Projectile.ai[2];
                Main.projectile[proj].localAI[1] = MathHelper.TwoPi * i / 8;
                Main.projectile[proj].localAI[0] = i;
            }
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = Projectile.direction;

            if (++Timer % 3 == 0)
            {
                int d = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 3) * 35), 1, 1, ModContent.DustType<ArenaDust>(),
                    0, 0, 0, Color.White, 0.75f);
                Main.dust[d].velocity = Vector2.Zero;

                int d2 = Dust.NewDust(Projectile.Center + Vector2.One.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2 - MathHelper.PiOver4) * (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 3) * 35), 1, 1, ModContent.DustType<ArenaDust>(),
    0, 0, 0, Color.White, 0.75f);
                Main.dust[d2].velocity = Vector2.Zero;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++) Dust.NewDust(Projectile.Center, 8, 8, DustID.SpectreStaff, Projectile.velocity.X * 5, Projectile.velocity.Y * 5);
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
        }
    }
}