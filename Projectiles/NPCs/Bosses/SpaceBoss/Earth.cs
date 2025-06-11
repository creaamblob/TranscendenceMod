using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class Earth : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 256;
            Projectile.height = 256;
            Projectile.timeLeft = 630;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }

        private Effect eff;
        private Asset<Texture2D> earth;
        public override bool PreDraw(ref Color lightColor)
        {
            if (earth == null)
                earth = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/EarthMap");

            SpriteBatch spriteBatch = Main.spriteBatch;

            if (!Main.dedServ)
            {
                eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/EarthHairShader", AssetRequestMode.ImmediateLoad).Value;

                Main.instance.GraphicsDevice.Textures[1] = earth.Value;
                eff.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 0.75f);
                eff.Parameters["uImageSize0"].SetValue(new Vector2(256, 256));
                eff.Parameters["uImageSize1"].SetValue(new Vector2(4180, 2560));



                TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, eff);


                //Draw the globe with shaders
                TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, Texture, 0f, Projectile.Center, null);
                

                TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);


            }

            return false;
        }

        public override void AI()
        {
            Player p = Main.player[Main.npc[(int)Projectile.ai[1]].target];
            if (p != null && p.active)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(p.Center) * 6f, 0.075f);


            if (Projectile.timeLeft < 30)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 30f);
        }
    }
}