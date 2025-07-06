using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class NucleusTarget : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 5;
            Projectile.hostile = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            Projectile.ai[2]++;

            if (NPC.AnyNPCs(ModContent.NPCType<ProjectNucleus>()))
                Projectile.timeLeft = 5;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            TranscendenceUtils.CircularAoETelegraph(sb, Projectile.Center, new Color(100, 30, 30), 2.5f, 16);
            TranscendenceUtils.DrawEntity(Projectile, Color.Red * 0.5f, Projectile.scale * MathHelper.Lerp(3f, 0f, Projectile.ai[2] / 75f), TranscendenceMod.ASSET_PATH + "/GrazeCircle", Main.GlobalTimeWrappedHourly * 3f, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Orange * 0.5f, Projectile.scale * 1.5f, TranscendenceMod.ASSET_PATH + "/GrazeCircle", Main.GlobalTimeWrappedHourly * 3f, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, 0f, Projectile.Center, null);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}