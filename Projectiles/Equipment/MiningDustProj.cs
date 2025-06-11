using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class MiningDustProj : ModProjectile
    {
        public float amount;
        public int MiningCD;
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
            Projectile.timeLeft = 70;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            TranscendenceUtils.DrawEntity(Projectile, new Color(0f, 0.33f, 1f) * 0.5f, Projectile.scale, "bloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White * 0.35f, Projectile.scale * 0.5f, "bloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale * 0.4f, Texture, 0, Projectile.Center, false, false, false);
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            amount = Main.rand.NextFloat(-6, 6);
        }
        public override void AI()
        {
            TranscendenceUtils.AnimateProj(Projectile, 4);
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(amount) / MathHelper.Lerp(0.125f, 12.5f, Projectile.timeLeft / 90f));

            if (MiningCD > 0)
                MiningCD--;

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
                    Player player = Main.player[Projectile.owner];

                    if (Projectile.owner == Main.myPlayer && tile.HasTile && player != null && tile != null && MiningCD == 0)
                    {
                        player.PickTile((int)(pos.X / 16), (int)(pos.Y / 16), 65);
                        MiningCD = 5;
                    }
                }
            }
        }
    }
}