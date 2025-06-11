using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class CondensedAir : ModProjectile
    {
        public ref float iHateGeomertry => ref Projectile.localAI[1];
        public Vector2 pos;
        public float Area;
        public int Number;

        public override string Texture => "TranscendenceMod/Miscannellous/Assets/GlowBloomNoBG";
        public int Timer = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 0.25f, Texture, true, true, 2f, -new Vector2(Projectile.width * 5, Projectile.height));

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override void AI()
        {
            Area = Projectile.ai[0];

            Vector2 area = new Vector2((float)(Math.Sin(++Projectile.ai[2] / 10) * Area));
            pos = Main.projectile[(int)Projectile.ai[1]].Top + iHateGeomertry.ToRotationVector2() * area;
            Projectile.Center = pos;
            iHateGeomertry += MathHelper.ToRadians(1);
        }
    }
}