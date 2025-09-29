using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Crean
{
    public class CreanStargazer : ModProjectile
    {
        public Vector2 pos;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 24;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 5;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player != null && player.active && !player.dead && player.HeldItem.type == ModContent.ItemType<CreanStaff>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
                Projectile.timeLeft = 5;

            Vector2 vec2 = Vector2.One.RotatedBy(Projectile.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 500f;


            if (Main.MouseWorld.Distance(player.Center) > 200)
                Projectile.ai[0] = Projectile.DirectionTo(player.Center + vec2).ToRotation();

            Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * Projectile.ai[1] / Projectile.ai[2] + TranscendenceWorld.UniversalRotation * 2f) * 75f;

            Projectile.Center = player.Center + new Vector2(vec.X, vec.Y / 2f).RotatedBy(Projectile.ai[0]);

            int timer = player.GetModPlayer<TranscendencePlayer>().CreanStaffClick;

            if (timer == 15)
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld).RotatedByRandom(0.1f),
                    ModContent.ProjectileType<CreanStargazerLaser>(), Projectile.damage, 2f, Projectile.owner, 0, Projectile.whoAmI, 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.VeryBasicProjOutline(Projectile, Texture, 2f, 0f, 0.5f, 0.75f, 0.5f, false);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);

            return false;
        }
    }
}