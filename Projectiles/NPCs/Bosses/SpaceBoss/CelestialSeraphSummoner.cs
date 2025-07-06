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
            Projectile.timeLeft = 60;

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

            local.GetModPlayer<TranscendencePlayer>().cameraPos = Projectile.Center - new Vector2(0, 150);
            local.GetModPlayer<TranscendencePlayer>().cameraModifier = true;

            starSize += 0.75f;
        }
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
            SpriteBatch sb = Main.spriteBatch;

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D sprite3 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/StarEffect").Value;
            Texture2D sprite4 = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/GlowBloom").Value;
            Vector2 pos = Projectile.Center - new Vector2(0, 150);

            sb.Draw(sprite3, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(340 * starSize), (int)(420 * starSize)), null,
                Color.Aqua * 0.5f, starSize / 4f, sprite3.Size() * 0.5f, SpriteEffects.None, 0);

            sb.Draw(sprite3, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(240 * starSize), (int)(320 * starSize)), null,
                Color.White, starSize / 2f, sprite3.Size() * 0.5f, SpriteEffects.None, 0);

            sb.Draw(sprite4, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(600 * starSize), (int)(600 * starSize)), null,
                Color.White, 0f, sprite4.Size() * 0.5f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

        }
        public override bool PreKill(int timeLeft)
        {
            NPC.NewNPC(Projectile.GetSource_Death(), (int)Projectile.Center.X, (int)Projectile.Center.Y - 500, ModContent.NPCType<CelestialSeraph>());

            return base.PreKill(timeLeft);
        }
    }
}