using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class WhiteDwarfProj : ModProjectile
    {
        public int YoyoFuse;
        public int ExplosionCD = 45;
        public int ExplosionCooloff = 0;
        public float BoomSize = 0.6f;
        public int Timer;
        Player player;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 675;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 20;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.aiStyle = 99;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            Vector2 Circle = new Vector2(-8, 0);
            int num2 = player.ownedProjectileCounts[Type];

            if (ExplosionCooloff > 0)
                ExplosionCooloff--;
            else ExplosionCD = 90;

            if (num2 == 2)
                Projectile.velocity += Main.rand.NextVector2Circular(1, 1) * 15;

            if (ExplosionCD < 4)
            {
                SoundEngine.PlaySound(SoundID.Item117 with { MaxInstances = 0, Volume = 0.15f, Pitch = -1f }, Projectile.Center);
                Projectile.localNPCHitCooldown = 3;
            }
            else Projectile.localNPCHitCooldown = 10;

            Timer++;
            if (Timer > ExplosionCD && ExplosionCD > 4)
            {
                SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion with { MaxInstances = 0 }, Projectile.Center);
                int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SpaceBossBombBlast>(),
                    (int)(Projectile.damage * 0.2f), 11, player.whoAmI, 1, 0.5f, 1);

                    Main.projectile[proj2].friendly = true;
                    Main.projectile[proj2].hostile = false;
                    Main.projectile[proj2].DamageType = DamageClass.Melee;
                /*for (int i = 0; i < 8; i++)
                {
                    int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(),
                        Projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 8) * 75,
                        Vector2.Zero, ModContent.ProjectileType<SpaceBossBombBlast>(), (int)(Projectile.damage * 0.2f), 11,
                        player.whoAmI, 1, 0.5f, 1);

                    Main.projectile[proj2].friendly = true;
                    Main.projectile[proj2].hostile = false;
                }*/
                Timer = 0;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion with { MaxInstances = 0}, Projectile.Center);
            ExplosionCooloff = 90;
            if (ExplosionCD > 4) ExplosionCD -= 4;

            if (Main.rand.NextBool(3) && ExplosionCD > 4)
            {
                target.AddBuff(BuffID.ShadowFlame, 120);
                target.AddBuff(BuffID.Oiled, 180);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.Orange * 0.66f, 2.15f, "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG",
                0, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (ExplosionCD < 5)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                TranscendenceUtils.DrawEntity(Projectile, Color.OrangeRed * 0.66f, 1.8f, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleAura",
                    MathHelper.ToRadians(Projectile.ai[2] * -5), Projectile.Center, null);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                TranscendenceUtils.DrawEntity(Projectile, Color.White, 0.4f, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHole2",
                    MathHelper.ToRadians(++Projectile.ai[2] * 3), Projectile.Center, null);

                TranscendenceUtils.DrawEntity(Projectile, Color.Black, 0.3f, "TranscendenceMod/Miscannellous/Assets/BlackHole/BlackHoleCenter",
                    MathHelper.ToRadians(Projectile.ai[2] * 2), Projectile.Center, null);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }

            return ExplosionCD > 10;
        }
    }
}