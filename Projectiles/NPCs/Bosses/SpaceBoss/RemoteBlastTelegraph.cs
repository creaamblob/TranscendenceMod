using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class RemoteBlastTelegraph : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/Trail2";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 72;

            Projectile.timeLeft = 90;
            Projectile.scale = 0f;

            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }
        public override bool? CanDamage() => false;

        public Vector2 npcPos;
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[1]];

            if (npc == null)
                return;

            npcPos = npc.Center;

            if (Projectile.timeLeft > 60 && Projectile.scale < 1f)
            {
                if (Projectile.ai[0] != 1f)
                    Projectile.Center = Main.player[npc.target].Center;
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 1f / 30f);
            }

            if (Projectile.timeLeft == 40)
            {
                if (Projectile.ai[0] == 1f)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(Main.player[npc.target].Center).RotatedByRandom(0.3f) * Main.rand.NextFloat(14f, 18f),
                            ModContent.ProjectileType<CosmicSphere>(), Projectile.damage, Projectile.knockBack, -1, 0, Projectile.ai[1]);
                    }
                }
                else
                {
                    for (int i = 0; i < 36; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2CircularEdge(8f, 8f) * Main.rand.NextFloat(1f, 1.75f),
                            ModContent.ProjectileType<CosmicSphere>(), Projectile.damage, Projectile.knockBack, -1, 0, Projectile.ai[1]);
                    }

                }

                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ImpurityBlast>(),
                    Projectile.damage, Projectile.knockBack, -1, 0, Projectile.ai[1]);
            }

            if (Projectile.timeLeft < 45 && Projectile.scale > 0f)
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0f, 1f / 45f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 pos = Vector2.Lerp(npcPos, Projectile.Center, 0.5f);
            float rot = npcPos.DirectionTo(Projectile.Center).ToRotation() + MathHelper.PiOver2;

            TranscendenceUtils.RestartSB(sb, BlendState.Additive, null);

            sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(250 * Projectile.scale), (int)(npcPos.Distance(Projectile.Center) * 1.5f)), null,
                    Color.Purple, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(140 * Projectile.scale), (int)(npcPos.Distance(Projectile.Center) * 1.5f)), null,
                Color.White, rot, sprite.Size() * 0.5f, SpriteEffects.None, 0);


            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale * 4f, "TranscendenceMod/Miscannellous/Assets/Portal", Projectile.timeLeft * 0.1f, Projectile.Center, null);


            TranscendenceUtils.RestartSB(sb, BlendState.AlphaBlend, null);


            return false;
        }
    }
}