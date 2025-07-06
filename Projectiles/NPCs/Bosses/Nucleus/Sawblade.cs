using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class Sawblade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.tileCollide = false;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 690;
            Projectile.hostile = true;
        }
        public override void AI()
        {
            Projectile.rotation += 0.175f;
            Vector2 vel = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel;

            bool InsideLiquid = false;

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile projectile = Main.projectile[p];

                if (projectile != null && projectile.active && projectile.ModProjectile is BloodLiquid liquid && liquid.InsideLiquid(Projectile))
                    InsideLiquid = true;
            }

            if (InsideLiquid)
                Projectile.velocity = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel * 0.66f;
            else Projectile.velocity = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel;

            NPC npc = Main.npc[(int)Projectile.ai[1]];

            if (npc == null || !npc.active)
                Projectile.Kill();

            if (++Projectile.ai[0] < 60)
            {
                Player player = Main.player[npc.target];
                Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel = npc.DirectionTo(player.Center) * 12f;


                float dist = Projectile.ai[0] > 30 ? 75f : Projectile.ai[0] * 2.5f;
                float rot = (Projectile.ai[0] < 30 ? 0f : (Projectile.ai[0] - 30) / 30f) + 0.2f;

                Projectile.Center = npc.Center + Vector2.One.RotatedBy(vel.ToRotation() - MathHelper.PiOver4 + Projectile.ai[2] * rot / 3f) * dist;

                return;
            }
            if (Projectile.ai[0] == 61)
                Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel = Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel.RotatedBy(Projectile.ai[2] / 3f);
            
            if (Main.rand.NextBool(20))
                SoundEngine.PlaySound(SoundID.Item22 with { MaxInstances = 0 }, Projectile.Center);

            Projectile.localAI[1]--;
            Projectile.localAI[2]--;

            if (npc.ModNPC is ProjectNucleus boss)
            {
                if ((Projectile.Center.X > (boss.Center.X + 875) || Projectile.Center.X < (boss.Center.X - 875)) && Projectile.timeLeft > 90 && Projectile.localAI[1] < 1)
                {
                    Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel.X *= -1f;
                    SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with { MaxInstances = 0 }, Projectile.Center);
                    Projectile.localAI[1] = 5;
                }

                if ((Projectile.Center.Y > (boss.Center.Y + 475) || Projectile.Center.Y < (boss.Center.Y - 475)) && Projectile.timeLeft > 90 && Projectile.localAI[2] < 1)
                {
                    Projectile.GetGlobalProjectile<TranscendenceProjectiles>().baseVel.Y *= -1f;
                    SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with { MaxInstances = 0 }, Projectile.Center);
                    Projectile.localAI[2] = 5;
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center) < (30 * Projectile.scale))
                return true;
            else return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override void OnKill(int timeLeft)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 8, DustID.TheDestroyer, Color.White, 1f, 2f, 4f);
        }
        public override bool PreDraw(ref Color lightColor)
        {

            if (Projectile.ai[0] < 60)
            {
                NPC npc = Main.npc[(int)Projectile.ai[1]];
                SpriteBatch spriteBatch = Main.spriteBatch;

                if (npc != null && npc.active)
                {
                    Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Nucleus/HealHeart_Line").Value;


                    spriteBatch.Draw(sprite, new Rectangle(
                        (int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), 24,
                        (int)(Projectile.Distance(npc.Center) * 2)), null,
                        Color.White, Projectile.DirectionTo(npc.Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
                }
            }

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White * 0.5f, Projectile.scale, Texture,
                true, true, 1f, Vector2.Zero, true, Projectile.rotation);


            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2, 1f, 0f, 0f, 0.375f, false);


            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}