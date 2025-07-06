using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class P2SupernovaSun : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;

            Projectile.width = 256;
            Projectile.height = 256;
            Projectile.timeLeft = 120;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
             
            ArtificialSun.DrawBreathingStar(Projectile, Projectile.scale, spriteBatch, true);
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        public override bool PreKill(int timeLeft)
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (npc != null && npc.active && npc.ModNPC is CelestialSeraph boss)
            {
                int p = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NucleusDeathGigaBoom>(), 0, 0f, -1, 0, npc.whoAmI);
                Main.projectile[p].timeLeft = boss.AttackDuration - 60;

                for (int i = 0; i < 256; i++)
                {
                    int p2 = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2Circular(48f, 48f), ModContent.ProjectileType<P2SupernovaProj>(), 110, 0f, -1, 0, Projectile.ai[1]);
                    Main.projectile[p2].timeLeft = boss.AttackDuration - 120;
                }
            }

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<P2SupernovaBlackhole>(), 0, 0f, -1, 0, Projectile.ai[1]);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NucleusDeathGigaBoom>(), 0, 0f);

            SoundEngine.PlaySound(ModSoundstyles.SeraphBomb);
            return base.PreKill(timeLeft);
        }

        public override void AI()
        {
            if (Projectile.scale < 90)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 90f);

            if (Main.npc[(int)Projectile.ai[1]].ModNPC is CelestialSeraph boss)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player p = Main.player[i];
                    if (p != null && p.active && p.Distance(Projectile.Center) < (Projectile.scale * 375f))
                        p.AddBuff(ModContent.BuffType<SunMelt>(), 2);
                }
            }
        }
    }
}