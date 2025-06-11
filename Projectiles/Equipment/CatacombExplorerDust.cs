using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class CatacombExplorerDust : ModProjectile
    {
        public float amount;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = -1;

            Projectile.ignoreWater = true;
            Projectile.timeLeft = 80;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, Color.Red * 0.35f, Projectile.scale, "bloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale * 0.35f, Texture, 0, Projectile.Center, false, false, false);
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            amount = Main.rand.NextFloat(-8, 8);
        }
        public override void AI()
        {
            TranscendenceUtils.AnimateProj(Projectile, 4);
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(amount) / MathHelper.Lerp(0.15f, 15f, Projectile.timeLeft / 80f));

            if (Projectile.timeLeft < 20)
            {
                Projectile.scale -= 1f / 20f;
                return;
            }

            for (int i = -16; i < 16; i++)
            {
                for (int j = -16; j < 16; j++)
                {
                    Vector2 pos = Projectile.Center + new Vector2(i, j);
                    Tile tile = Framing.GetTileSafely((int)pos.X / 16, (int)pos.Y / 16);
                    if (Projectile.owner == Main.myPlayer && (tile.TileType == TileID.CrackedBlueDungeonBrick || tile.TileType == TileID.CrackedGreenDungeonBrick || tile.TileType == TileID.CrackedPinkDungeonBrick || tile.TileType == TileID.Spikes)
                        && tile.HasTile && Main.player[Projectile.owner] != null && tile != null)
                    {
                        Main.player[Projectile.owner].PickTile((int)(pos.X / 16), (int)(pos.Y / 16), 150);
                    }
                }
            }
        }
    }
}