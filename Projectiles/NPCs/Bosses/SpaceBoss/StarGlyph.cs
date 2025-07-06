using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class StarGlyph : ModProjectile
    {
        public float Alpha = 1;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 480;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().ModUnparryable = true;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White * Alpha;
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (npc == null) return;

            Projectile.velocity = Projectile.DirectionTo(npc.Center) * Projectile.ai[0];
            if (++Projectile.localAI[2] > 60)
            {
                if (Projectile.Distance(npc.Center) < 50) Projectile.damage = 0;
                Projectile.scale -= 0.0033f;
            }

            /*if (npc.ai[3] > 530)
            {
                Projectile.velocity *= 0.95f;
                Alpha -= 0.1f;
            }*/

            bool near = Projectile.Distance(npc.Center) < Projectile.ai[2];
            if (Alpha < 0 || near)
            {
                if (near)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 500, 40f, -1, 0f, 0.75f, 1f);

                    SoundEngine.PlaySound(ModSoundstyles.SeraphBomb with { MaxInstances = 0 }, npc.Center);
                }
                Projectile.Kill();
            }

        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 3);
            TranscendenceUtils.NoExpertProjDamage(Projectile);
            if (Projectile.ai[0] != 3) TranscendenceUtils.DustRing(Projectile.Center, 10, DustID.BlueFairy, 3, Color.White, 1.25f);
        }
        public override bool CanHitPlayer(Player target) => Alpha > 0.75f;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uOpacity"].SetValue(1f);
            eff.Parameters["uSaturation"].SetValue(1f);

            eff.Parameters["uRotation"].SetValue(1f);
            eff.Parameters["uTime"].SetValue(1f);
            eff.Parameters["uDirection"].SetValue(1f);
            float dist = 2f;

            TranscendenceUtils.DrawEntity(Projectile, Color.Blue * 0.75f, 1.75f * Projectile.scale, "bloom", 0, Projectile.Center, null);
            TranscendenceUtils.DrawEntity(Projectile, Color.Aqua, Projectile.scale, "bloom", 0, Projectile.Center, null);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * 6) * dist;
                TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, Texture, Projectile.rotation, pos, false, false, false);
            }
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            return true;
        }
    }
}