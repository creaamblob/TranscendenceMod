using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class CometLaser : ModProjectile
    {
        public Vector2 pos;
        public override string Texture => "TranscendenceMod/NPCs/SpaceBiome/Worm/SpaceWorm_Head";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 40;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.width = 44;
            Projectile.height = 34;
            Projectile.timeLeft = 150;
            ProjectileID.Sets.TrailCacheLength[Type] = 80;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            AIType = ProjectileID.Bullet;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White * wormAlpha;
        public override void OnSpawn(IEntitySource source)
        {
            
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }

        public float wormAlpha => Projectile.timeLeft > 45 ? 1f : MathHelper.Lerp(0f, 1f, Projectile.timeLeft / 45f);

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/SpaceBiome/Worm/SpaceWorm_Body").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>("TranscendenceMod/NPCs/SpaceBiome/Worm/SpaceWorm_Tail").Value;
            Texture2D sprite3 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Portal").Value;

            Vector2 origin = new Vector2(sprite.Width * 0.5f, Projectile.height * 0.5f);

            for (int i = 1; i < ((Projectile.oldPos.Length - 1) / 3); i++)
            {
                Vector2 pos = Projectile.oldPos[i * 3] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);

                Main.EntitySpriteDraw(sprite, pos, null, Color.White * wormAlpha, Projectile.oldRot[i * 3] + MathHelper.PiOver2, origin, Projectile.scale, SpriteEffects.None);
            }

            Vector2 pos2 = Projectile.oldPos[79] - Main.screenPosition + origin + new Vector2(0, Projectile.gfxOffY);
            Main.EntitySpriteDraw(sprite2, pos2, null, Color.White * wormAlpha, Projectile.oldRot[79] + MathHelper.PiOver2, sprite2.Size() * 0.5f, Projectile.scale, SpriteEffects.None);

            float portalAlpha = Projectile.ai[2] < 50 ? 1f : MathHelper.Lerp(1f, 0f, (Projectile.ai[2] - 50) / 30f);
            for (int i = 0; i < 80; i++)
            {
                if (i < 78 && Projectile.oldPos[i] != Vector2.Zero && Projectile.oldPos[i + 1] == Vector2.Zero)
                {
                    Main.EntitySpriteDraw(sprite3, Projectile.oldPos[i] - Main.screenPosition, null, Color.White * 0.5f * portalAlpha, TranscendenceWorld.UniversalRotation * -6f, sprite3.Size() * 0.5f, Projectile.scale * 3f, SpriteEffects.None);
                    Main.EntitySpriteDraw(sprite3, Projectile.oldPos[i] - Main.screenPosition, null, Color.White * portalAlpha, TranscendenceWorld.UniversalRotation * 3f, sprite3.Size() * 0.5f, Projectile.scale * 2f, SpriteEffects.None);
                    break;
                }
            }

            return true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (targetHitbox.Distance(Projectile.oldPos[i]) < 16 && wormAlpha == 1f)
                    return true;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.spriteDirection = (int)Projectile.velocity.ToRotation();

            if (++Projectile.ai[2] > 10 && Projectile.ai[2] < 150)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player != null && player.active && player.Distance(Projectile.Center) < 1200)
                    {
                        if (Projectile.ai[2] % 4 == 0)
                        {
                            pos = player.Center + Vector2.One.RotatedBy(Projectile.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver4) * 1500;
                        }
                        Vector2 targetVelocity = Projectile.DirectionTo(pos) * 14f;
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.03375f);
                    }
                }
            }
        }
    }
}