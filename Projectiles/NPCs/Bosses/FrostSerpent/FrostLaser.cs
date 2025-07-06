using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.FrostSerpent
{
    public class FrostLaser : ModProjectile
    {
        public float rot;
        public Vector2 Center;
        public NPC ChosenNPC;
        public Player player;
        public Vector2 Pos;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/BloomLine2";

        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
 
            Projectile.tileCollide = false;
            Projectile.scale = 0.125f;
            Projectile.timeLeft = 140;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            
            if (!ChosenNPC.active)
                Projectile.Kill();

            if (Projectile.timeLeft < 135 && ChosenNPC != null && ChosenNPC.active)
            {
                Center = ChosenNPC.Center;
                Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 100f;
                ChosenNPC.rotation = rot + MathHelper.PiOver2 + MathHelper.PiOver4;
            }

            rot += 0.05f;

            if (Projectile.timeLeft < (140 - 25) && Projectile.timeLeft > 30 && Projectile.scale < 1f)
                Projectile.scale += 0.05f;

            if (Projectile.timeLeft < 15)
                Projectile.scale -= 1f / 15f;

            if (Projectile.timeLeft == (140 - 25))
            {
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                    new Vector2(Main.rand.NextFloatDirection()), 45, 5, 5, -1, null));

                SoundEngine.PlaySound(SoundID.Item67 with { MaxInstances = 0, Volume = 0.4f });
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;

            NPC npc = Main.npc[(int)Projectile.ai[1]];

            if (npc != null && npc.active)
            {
                player = Main.player[npc.target];
                ChosenNPC = npc;
            }

            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Center, Center + Vector2.One.RotatedBy(rot) * 2000, 25, ref reference))
                return true;

            else return false;
        }
        public override bool? CanDamage() => Projectile.timeLeft < (140 - 30) && Projectile.timeLeft > 15;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4000;
            SpriteBatch sb = Main.spriteBatch;

            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 pos = Center + Vector2.One.RotatedBy(rot) * 50f;
            Vector2 pos2 = Projectile.Center + Vector2.One.RotatedBy(rot) * 1500f;

            //Request the Effect
            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/MovingNoiseTrans", AssetRequestMode.ImmediateLoad).Value;
            //Apply Shader Texture
            Texture2D shaderImage = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SpiritShader").Value;
            Main.instance.GraphicsDevice.Textures[1] = shaderImage;

            eff.Parameters["uImageSize0"].SetValue(shaderImage.Size());
            eff.Parameters["uImageSize1"].SetValue(shaderImage.Size());
            eff.Parameters["uTime"].SetValue(rot.ToRotationVector2().X);
            eff.Parameters["yChange"].SetValue(rot.ToRotationVector2().Y);

            eff.Parameters["useExtraCol"].SetValue(false);
            eff.Parameters["useAlpha"].SetValue(false);

            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(250 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                Color.Blue * 0.66f * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);


            sb.End();
            sb.Begin(default, BlendState.Additive, default, default, default, eff, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 5; i++)
            {
                sb.Draw(sprite, new Rectangle(
                    (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(66 * Projectile.scale), (int)(pos.Distance(pos2) * 2f)), null,
                    Color.White * Projectile.scale, pos.DirectionTo(pos2).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);
            }

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}