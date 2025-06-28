using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class NucleusMine : ModProjectile
    {
        public Player player;
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.tileCollide = false;

            Projectile.aiStyle = -1;
            Projectile.hostile = true;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.95f;

            if (NPC.AnyNPCs(ModContent.NPCType<ProjectNucleus>()))
                Projectile.timeLeft = 5;

            if (Projectile.localAI[0] < 1f)
                Projectile.localAI[0] += 1f / 30f;

            if (Projectile.ai[2] == 1)
            {
                SoundEngine.PlaySound(ModSoundstyles.SeraphBomb with { MaxInstances = 0 }, Projectile.Center);

                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<NucleusMineBlast>(), Projectile.damage, Projectile.knockBack, -1, 0, Projectile.ai[1]);

                Projectile.Kill();
            }
        }
        public override bool? CanDamage() => false;
        public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.localAI[0];
        public override bool PreDraw(ref Color lightColor)
        {
            //Coordinates
            int x = (int)(Projectile.Center.X - 282) - (int)Main.screenPosition.X;
            int y = (int)(Projectile.Center.Y - 282) - (int)Main.screenPosition.Y;

            //Destination Rectangle
            Rectangle rec = new Rectangle(x, y, 564, 564);
            Rectangle rec2 = new Rectangle(x + (564 / 4), y + (564 / 4), 564 / 2, 564 / 2);


            //Request effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/Pixelation", AssetRequestMode.ImmediateLoad).Value;

            eff.Parameters["uImageSize1"].SetValue(new Vector2(564, 564));
            eff.Parameters["maxColors"].SetValue(16);


            //The Sprite
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;

            SpriteBatch spriteBatch = Main.spriteBatch;


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, eff);


            spriteBatch.Draw(sprite, rec, null, Color.DarkGray * 0.75f * Projectile.localAI[0]);
            spriteBatch.Draw(sprite, rec2, null, Color.DarkGray * Projectile.localAI[0]);


            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);

            return base.PreDraw(ref lightColor);
        }
    }
}