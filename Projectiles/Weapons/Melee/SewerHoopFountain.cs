using FullSerializer;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Melee
{
    public class SewerHoopFountain : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 96;

            Projectile.timeLeft = 120;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 45)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 45f);

            int dustType = DustID.DungeonWater;

            Dust.NewDustPerfect(Projectile.Center + new Vector2((float)Math.Sin(TranscendenceWorld.UniversalRotation * 5f) * 25f, 0f), dustType, new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), -5f), 0, Color.White, Main.rand.NextFloat(1f, 1.5f));
            Dust.NewDustPerfect(Projectile.Center + new Vector2((float)Math.Sin(TranscendenceWorld.UniversalRotation * 5f) * -25f, 0f), dustType, new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), -5f), 0, Color.White, Main.rand.NextFloat(1f, 1.5f));
        }
    }
}