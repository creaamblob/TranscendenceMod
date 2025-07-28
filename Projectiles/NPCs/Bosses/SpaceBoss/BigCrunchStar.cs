using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class BigCrunchStar : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.timeLeft = 600;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(4f);

            if (Projectile.ai[2] > 0)
            {
                Projectile.ai[2]++;

                if (Projectile.GetGlobalProjectile<TranscendenceProjectiles>().StupidInt > 0)
                    Projectile.GetGlobalProjectile<TranscendenceProjectiles>().StupidInt--;

                NPC npc = Main.npc[(int)Projectile.ai[1]];

                if (npc == null)
                    return;

                Projectile.velocity = Projectile.DirectionTo(npc.Center) * MathHelper.Lerp(2f, 10f, Projectile.ai[2] / 105f);

                if (Projectile.Distance(npc.Center) < 175)
                {
                    SoundEngine.PlaySound(new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Hurt/CelestialSeraphShield")
                    {
                        Volume = 0.33f,
                        Pitch = 2f,
                        MaxInstances = 0
                    }, Projectile.Center);


                    for (int i = 0; i < 4; i++)
                        Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<ArenaDustNoCenter>(), Main.rand.NextVector2Circular(4f, 4f), 0, new Color(50, 50, 75), 1f);

                    Projectile.Kill();
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
            Projectile.rotation = MathHelper.ToRadians(Projectile.ai[0] * 4f);
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            DrawBCStar(Projectile);
            return false;
        }
        public static void DrawBCStar(Projectile projectile)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D starSprite = TextureAssets.Item[ItemID.ManaCloakStar].Value;

            float fade = projectile.ai[2] / 105f;
            Color col = Color.Lerp(Main.hslToRgb(projectile.ai[0] * 2f, 1f, 0.5f), new Color(50, 50, 75), fade);
            float size = projectile.GetGlobalProjectile<TranscendenceProjectiles>().StupidInt * 0.025f;

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.Additive, null);

            TranscendenceUtils.DrawEntity(projectile, col * 0.75f, 1f + size, TranscendenceMod.ASSET_PATH + "/GlowBloom", projectile.rotation, projectile.Center, null);
            TranscendenceUtils.DrawEntity(projectile, Color.White * 0.75f * (1 - fade), 0.5f + size, TranscendenceMod.ASSET_PATH + "/GlowBloom", projectile.rotation, projectile.Center, null);

            TranscendenceUtils.DrawEntity(projectile, col, projectile.scale * 1.25f, starSprite, projectile.rotation, projectile.Center, null);

            TranscendenceUtils.RestartSB(spriteBatch, BlendState.AlphaBlend, null);
        }
    }
}