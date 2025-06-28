using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class GloveThrowItem : ModProjectile
    {
        Texture2D sprite = null;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.ignoreWater = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 450;
            Projectile.tileCollide = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return targetHitbox.Distance(Projectile.Center) < 32;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            if (player != null)
            {
                sprite = TextureAssets.Item[player.HeldItem.type].Value;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (sprite != null)
                TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, sprite, Projectile.rotation, Projectile.Center, null);
            return false;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(15);
            Projectile.velocity.Y += 0.5f;

            for (int i = -16; i < 16; i++)
            {
                for (int j = -16; j < 16; j++)
                {
                    Vector2 pos = Projectile.Center + new Vector2(i, j);
                    Tile tile = Framing.GetTileSafely((int)pos.X / 16, (int)pos.Y / 16);
                    if (Projectile.owner == Main.myPlayer && tile.HasTile && Main.player[Projectile.owner] != null && Projectile.ai[0] > 0 && tile != null)
                    {
                        Projectile.timeLeft -= 5;
                        Main.player[Projectile.owner].PickTile((int)(pos.X / 16), (int)(pos.Y / 16), (int)Projectile.ai[0]);
                    }
                    if (Projectile.owner == Main.myPlayer && tile.HasTile && Main.player[Projectile.owner] != null && Projectile.ai[1] > 0 && tile != null && Main.tileAxe[tile.TileType])
                    {
                        Main.player[Projectile.owner].PickTile((int)(pos.X / 16), (int)(pos.Y / 16), (int)Projectile.ai[1]);
                    }
                }
            }
        }
    }
}