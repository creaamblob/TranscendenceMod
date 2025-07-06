using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class SpaceVaccuumBlackHole : ModProjectile
    {
        public int Timer;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 150;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 60;
            Projectile.penetrate = -1;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.ownerHitCheck = true;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;

            Projectile.friendly = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.GetGlobalNPC<TranscendenceNPC>().Unmovable) target.velocity += target.DirectionTo(Main.player[Projectile.owner].Center) * (damageDone / 25);
            target.AddBuff(ModContent.BuffType<SpaceDebuff>(), 90);
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Main.player[Projectile.owner].ownedProjectileCounts[Type] > 0) Projectile.Kill();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 pos = Main.player[Projectile.owner].Center;
            float reference = float.NaN;
            for (int i = -13; i < 13; i++)
            {
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                    pos, pos + Vector2.One.RotatedBy(pos.DirectionTo(Projectile.Center).ToRotation() - MathHelper.PiOver4 + MathHelper.ToRadians(i)) * 440,
                    Projectile.scale, ref reference))
                {
                    return true;
                }
            }
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.controlUseItem && !player.dead && player.statMana > player.HeldItem.mana) Projectile.timeLeft = 5;
            Vector2 pos = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 305;
            Vector2 pos2 = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 700;
            Projectile.Center = pos;
            Projectile.rotation = Projectile.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2;

            if (Timer < 127)
                Timer++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 pos2 = player.Center + Vector2.One.RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation() - MathHelper.PiOver4) * 500;
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.NonPremultiplied, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawEntity(Projectile, Color.Orange, 1.25f, "TranscendenceMod/Miscannellous/Assets/VaccuumTrail",
                Projectile.rotation, player.Center + player.DirectionTo(pos2) * 305, null);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return base.PreDraw(ref lightColor);
        }
    }
}