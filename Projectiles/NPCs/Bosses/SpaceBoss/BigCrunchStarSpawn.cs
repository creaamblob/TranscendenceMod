using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class BigCrunchStarSpawn : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
            Projectile.rotation = MathHelper.ToRadians(Projectile.ai[0] * 4f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            BigCrunchStar.DrawBCStar(Projectile);
            return false;
        }
    }
}