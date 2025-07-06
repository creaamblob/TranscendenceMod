using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.Equipment.Tools
{
    public class GiantAnchorProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Tools/GiantAnchor";

        Player player;
        bool Attached;
        bool GenuineFall;

        Vector2 ownerPos;
        Vector2 pos;
        Vector2 angleToOwner;
        float extenderRot;
        float distance;

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 60;
            Projectile.tileCollide = true;
        }
        public override Color? GetAlpha(Color lightColor) => lightColor;
        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> sprite = TextureAssets.Chain22;

            ownerPos = player.MountedCenter;
            pos = Projectile.Center;
            angleToOwner = ownerPos - pos;
            extenderRot = angleToOwner.ToRotation() - MathHelper.PiOver2;
            distance = angleToOwner.Length();

            Rectangle? chainSourceRectangle = null;
            float chainHeightAdjustment = 0f;

            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : sprite.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
                chainSegmentLength = 10;

            while (chainSegmentLength >= 0 && distance > 20 && !float.IsNaN(distance))
            {
                angleToOwner /= distance;
                angleToOwner *= sprite.Height();

                pos += angleToOwner;
                angleToOwner = ownerPos - pos;
                distance = angleToOwner.Length();

                Main.EntitySpriteDraw(sprite.Value, pos - Main.screenPosition, sprite.Value.Bounds, Lighting.GetColor((int)pos.X / 16, (int)(pos.Y / 16f)), extenderRot, sprite.Size() * 0.5f, 1, SpriteEffects.None, 0);
            }
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0.9f;
            if (++Projectile.localAI[0] > 2)
            {
                Projectile.velocity = Vector2.Zero;
                Attached = true;
            }
            return false;
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            if (!player.active || player.dead)
                Projectile.Kill();

            if (++Projectile.localAI[1] > 15 && !Attached)
                GenuineFall = true;

            if (Attached && Projectile.localAI[1] > 300 && Projectile.Distance(player.Center) > 2500 || Projectile.localAI[2] == 1)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
                Projectile.Center = Projectile.Center.MoveTowards(player.Center, 15);
                Projectile.ai[2] = 1;
                Projectile.localAI[2] = 1;

               if (player.Distance(Projectile.Center) < 50)
                    Projectile.Kill();
            }
            if (Attached)
            {
                if (GenuineFall)
                {
                    player.velocity += player.DirectionTo(Projectile.Center) * 55;
                    player.slowFall = true;
                }
                if (player.Distance(Projectile.Center) < 50)
                {
                    Projectile.localAI[2] = 1;
                    Projectile.tileCollide = false;
                }
            }
            else Projectile.velocity.Y += 2;
        }
    }
}