using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons.Magic;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class LostHeadProj : ModProjectile
    {
        public int Timer;
        public int dir;
        public override string Texture => "TranscendenceMod/NPCs/Miniboss/HeadlessZombie_Head_Boss";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DismountsPlayersOnHit[Type] = true;
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float amount = 1.5f;
            int cd = 5;
            if (Projectile.ai[2] < 150)
                Projectile.ai[2] += amount;

            if (Projectile.ai[2] < 5)
                dir = player.direction;
            else player.direction = dir;

            Vector2 pos = new Vector2(0, Projectile.ai[2]).RotatedBy(MathHelper.ToRadians(++Projectile.ai[1] * 6f));
            Projectile.Center = player.Center + new Vector2(pos.X / 8f, pos.Y).RotatedBy(MathHelper.PiOver4 * -4f * player.direction);
            Dust.NewDustPerfect(Projectile.Bottom, DustID.Blood, new Vector2(0, 5), 0, default, 1.5f);

            if (player.channel) Projectile.timeLeft = 5;
            if (player.HeldItem.type != ModContent.ItemType<LostHead>() || player.statMana == 0)
                Projectile.Kill();

            Projectile.spriteDirection = player.direction;

            if (++Timer > cd)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(0, Main.rand.NextFloat(-Projectile.ai[2] / 2f, Projectile.ai[2] / 2f)),
                    new Vector2(dir * Main.rand.NextFloat(6f, 8.75f), 0f), ProjectileID.NailFriendly, Projectile.damage, 1, player.whoAmI);
                Main.projectile[proj].DamageType = DamageClass.Magic;
                Main.projectile[proj].extraUpdates = 1;
                Timer = 0;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}