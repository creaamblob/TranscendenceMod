using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.NPCs.Boss.Seraph;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss
{
    public class CelestialSeraphSummoner : ModProjectile
    {
        public int spawnTime = 450;

        public float starSize;
        public float rot;
        public Vector2 Center;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 900;

            Projectile.friendly = false;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver2 - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;
        }
        public override void AI()
        {
            Projectile.GetGlobalProjectile<TranscendenceProjectiles>().CanBeErased = false;
            Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 10;

            Player local = Main.LocalPlayer;

            local.GetModPlayer<TranscendencePlayer>().cameraPos = Projectile.Center - new Vector2(0, 200);
            local.GetModPlayer<TranscendencePlayer>().cameraModifier = true;

            Projectile.ai[2] += 2;
            float x = (float)Math.Sin(TranscendenceWorld.UniversalRotation * 5f) * 50f;
            Dust.NewDustPerfect(Projectile.Center - new Vector2(x, Projectile.ai[2]), ModContent.DustType<ArenaDust>(), Vector2.Zero, 0, Color.Gold, 1f);
            Dust.NewDustPerfect(Projectile.Center - new Vector2(-x, Projectile.ai[2]), ModContent.DustType<ArenaDust>(), Vector2.Zero, 0, Color.Gold, 1f);

            if (Projectile.ai[2] > 750)
            {
                starSize += 0.75f;
                if (Projectile.ai[2] > 800)
                    Projectile.Kill();
            }

        }
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine3").Value;

            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 50f;
            Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(rot) * 2000f;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphForcefieldShader").Value;

            Projectile.localAI[2] += 0.025f;

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.ai[2] < 110)
            {
                float a = Projectile.timeLeft > 180 ? MathHelper.Lerp(0.5f, 0f, (Projectile.timeLeft - 240) / 60f) : 1f;

                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), 175, (int)(pos.Distance(pos2) * 2f)), null,
                    Color.Gold * 0.66f * a, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;
            eff.Parameters["uImageSize1"].SetValue(new Vector2(shaderImage.Width * 2f, shaderImage.Height * 0.2f));

            eff.Parameters["uTime"].SetValue(Projectile.localAI[2]);
            eff.Parameters["yChange"].SetValue(Projectile.localAI[2]);

            eff.Parameters["useAlpha"].SetValue(false);
            eff.Parameters["useExtraCol"].SetValue(true);
            eff.Parameters["extraCol"].SetValue(new Vector3(1f, 0.66f, 0f));

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 8; i++)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(80 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(30 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            if (Projectile.ai[2] > 750)
            {
                Texture2D sprite3 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/StarEffect").Value;
                Texture2D sprite4 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
                Vector2 pos3 = Projectile.Center - new Vector2(0, 750);

                sb.Draw(sprite3, new Rectangle(
                    (int)(pos3.X - Main.screenPosition.X), (int)(pos3.Y - Main.screenPosition.Y), (int)(340 * starSize), (int)(420 * starSize)), null,
                    Color.Aqua * 0.5f, starSize / 4f, sprite3.Size() * 0.5f, SpriteEffects.None, 0);

                sb.Draw(sprite3, new Rectangle(
                    (int)(pos3.X - Main.screenPosition.X), (int)(pos3.Y - Main.screenPosition.Y), (int)(240 * starSize), (int)(320 * starSize)), null,
                    Color.White, starSize / 2f, sprite3.Size() * 0.5f, SpriteEffects.None, 0);

                sb.Draw(sprite4, new Rectangle(
                    (int)(pos3.X - Main.screenPosition.X), (int)(pos3.Y - Main.screenPosition.Y), (int)(600 * starSize), (int)(600 * starSize)), null,
                    Color.White, 0f, sprite4.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

        }
        public override bool PreKill(int timeLeft)
        {
            NPC.NewNPC(Projectile.GetSource_Death(), (int)Projectile.Center.X, (int)Projectile.Center.Y - 500, ModContent.NPCType<CelestialSeraph>());

            if (Main.netMode == 0)
            {
                Main.NewText(Language.GetTextValue("Announcement.HasAwoken", Language.GetTextValue("Mods.TranscendenceMod.NPCs.CelestialSeraph.DisplayName")), 175, 75, 255);
            }

            /*if (Main.netMode == 2)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken",
                [
                                    NetworkText.FromKey("Mods.TranscendenceMod.NPCs.CelestialSeraph.DisplayName", Array.Empty<object>())
                ]), new Color(175, 75, 255), -1);
            }*/
            return base.PreKill(timeLeft);
        }
    }
}