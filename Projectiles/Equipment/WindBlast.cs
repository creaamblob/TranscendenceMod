using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class WindBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            //Triple damage in Master Mode and Double in Expert
            Projectile.damage *= Main.masterMode ? 3 : Main.expertMode ? 2 : 1;
        }
        public override void PostDraw(Color lightColor)
        {
            TranscendenceUtils.AnimateProj(Projectile, 3);
            TranscendenceUtils.DrawProjAnimated(Projectile, lightColor, 1, $"{Texture}", Projectile.rotation, Vector2.Zero, true, true, true);
        }
    }
}