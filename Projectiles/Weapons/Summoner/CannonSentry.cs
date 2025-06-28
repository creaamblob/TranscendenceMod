using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using static Terraria.Player;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class CannonSentry : ModProjectile
    {
        public int AttackTimer;
        public int AttackDelay = 180;
        public float CannonRotation;
        public bool InRange;
        public bool Moving;
        public bool HasTarget;
        public readonly int cannonball = ModContent.ProjectileType<CannonBall>();
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.sentry = true;
            Projectile.friendly = true;

            Projectile.width = 54;
            Projectile.height = 38;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
        }

        public override bool? CanDamage() => false;

        public override Color? GetAlpha(Color lightColor) => lightColor;

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects se = SpriteEffects.None;
            if (!HasTarget) se = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            TranscendenceUtils.DrawEntity(Projectile, lightColor, Projectile.scale, $"{Texture}_Cannon", CannonRotation, Projectile.Center - new Vector2(1, 7),
                null, se);
            return base.PreDraw(ref lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            player.UpdateMaxTurrets();
        }
        public override bool OnTileCollide(Vector2 oldVelocity) => false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Moving) Projectile.velocity.Y = 5;

            HasTarget = false;
            Projectile.tileCollide = true;

            if (player.dead || !player.active)
                return;

            if (player.Center.Between(Projectile.Center - new Vector2(70, player.height), Projectile.Center +  new Vector2(70, player.height)))
                InRange = true;
            else InRange = false;

            if (InRange && Main.mouseRight && player.GetModPlayer<TranscendencePlayer>().Cannon == null)
            {
                player.GetModPlayer<TranscendencePlayer>().Cannon = Projectile;
                if (player.GetModPlayer<TranscendencePlayer>().Cannon == Projectile) Moving = true;
            }
            if (Moving && player.GetModPlayer<TranscendencePlayer>().Cannon != Projectile)
            {
                Projectile.position.Y -= 4;
                Moving = false;
            }
            if (Moving && Main.mouseRightRelease)
            {
                Projectile.position.Y -= 4;
                Moving = false;
            }

            if (Moving)
                CannonRotation = 0;

            if (Moving && InRange && player.GetModPlayer<TranscendencePlayer>().Cannon == Projectile)
            {
                player.SetCompositeArmFront(true, CompositeArmStretchAmount.Full, -MathHelper.PiOver2 * player.direction);
                player.SetCompositeArmBack(true, CompositeArmStretchAmount.Full, -MathHelper.PiOver2 * player.direction);

                player.heldProj = Projectile.whoAmI;
                player.GetModPlayer<TranscendencePlayer>().MovingCannon = 5;

                Projectile.tileCollide = false;
                Projectile.direction = player.direction;
                Projectile.spriteDirection = Projectile.direction;
                Projectile.position.X = player.Center.X + (player.direction > 0 ? 5 : -60);
                Projectile.position.Y = player.Center.Y - (Projectile.height / 2);
            }


            int target = Projectile.FindTargetWithLineOfSight(1250);

            if (target != -1 && player.Distance(Projectile.Center) < 1000 && !Moving)
            {
                NPC npc = Main.npc[target];
                if (player.MinionAttackTargetNPC != -1)
                    npc = Main.npc[player.MinionAttackTargetNPC];
                HasTarget = true;
                CannonRotation = Projectile.DirectionTo(npc.Center).ToRotation(); // + (npc.Center.X > Projectile.Center.X ? -MathHelper.PiOver4 : MathHelper.PiOver4)

                if (++AttackTimer > AttackDelay)
                {
                    Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(CannonRotation - MathHelper.PiOver4) * 500;
                    SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

                    if (player.Distance(Projectile.Center) < 500)
                    {
                        Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                            new Vector2(Main.rand.NextFloatDirection()), MathHelper.Lerp(70f, 2f, player.Distance(Projectile.Center) / 500f), 5, 5, -1, null));
                    }

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(pos - new Vector2(0, 75)) * 25, cannonball, Projectile.damage, Projectile.knockBack, player.whoAmI);
                    AttackTimer = 0;
                }
            }
        }
    }
}