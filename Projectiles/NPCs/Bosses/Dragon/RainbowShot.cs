using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class RainbowShot : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 26;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }

        private Color col;

        public override bool PreDraw(ref Color lightColor)
        {
            col = Main.hslToRgb(Projectile.ai[2], 1f, 0.5f);
            SpriteBatch spriteBatch = Main.spriteBatch;

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            TranscendenceUtils.DrawEntity(Projectile, col * 0.66f, Projectile.scale * 0.25f, TranscendenceMod.ASSET_PATH + "/GlowBloom", Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, col, Projectile.scale, Texture, Projectile.rotation, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale * 0.66f, Texture, Projectile.rotation, Projectile.Center, null);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++) Dust.NewDust(Projectile.Center, 8, 8, ModContent.DustType<ArenaDust>(), 0f, 0f, 0, col);
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
        }
    }
}