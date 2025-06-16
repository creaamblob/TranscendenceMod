using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Modifiers
{
    public class SentryBlackhole : ModProjectile
    {
        public float rot;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.ArmorPenetration = 9999;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;

            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return targetHitbox.Distance(Projectile.Center) < 100;
        }
        public override void AI()
        {
            rot += 0.05f;
            SoundEngine.PlaySound(SoundID.Item117 with { MaxInstances = 0, Volume = 0.1f, Pitch = -1f }, Projectile.Center);
            Projectile proj = Main.projectile[(int)Projectile.ai[0]];
            if (proj == null || !proj.active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.hide = true;
            Projectile.Center = proj.Center;

            for (int i = 0; i < Main.maxItems; i++)
            {
                Item item = Main.item[i];
                if (item == null)
                    return;

                if (item.Distance(Projectile.Center) < 450)
                    item.velocity += item.DirectionTo(Projectile.Center) * 2f;
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            BlackholeDrawer.DrawBlackhole(Projectile, 1f, spriteBatch);

            return false;
        }
    }
}