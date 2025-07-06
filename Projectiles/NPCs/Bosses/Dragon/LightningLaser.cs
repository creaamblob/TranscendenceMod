using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Dragon
{
    public class LightningLaser : ModProjectile
    {
        public int time = 600;
        public float rot;
        public Vector2 Center;
        public NPC ChosenNPC;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.aiStyle = -1;
            Projectile.scale = 0f;
 
            Projectile.tileCollide = false;
            Projectile.scale = 0.2f;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 300;

            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            
            if (Projectile.timeLeft < (time - 5) && ChosenNPC != null && ChosenNPC.active)
            {
                Center = ChosenNPC.Center;
                Projectile.Center = Center + Vector2.One.RotatedBy(rot) * 100;
            }

            for (float f = 0; f < 35; f++)
            {
                Lighting.AddLight(Vector2.SmoothStep(Center, Center + Vector2.One.RotatedBy(rot) * 1000, f / 35f), 0f, 0.025f * Projectile.scale, 0.1f * Projectile.scale);
            }

            if (Projectile.timeLeft == (time - 60))
            {
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                    new Vector2(Main.rand.NextFloatDirection()), 120, 5, 5, -1, null));

                SoundEngine.PlaySound(SoundID.Thunder with { MaxInstances = 0 });
                Projectile.scale = Projectile.ai[0];
            }

            if (Projectile.timeLeft > (time - 120) && Projectile.timeLeft < (time - 60))
                Projectile.scale += 0.1f;

            if (Projectile.timeLeft < 30 && Projectile.scale > 0)
                Projectile.scale -= 0.4f;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Center = Projectile.Center;
            rot = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            Projectile.velocity = Vector2.Zero;

            NPC npc = Main.npc[(int)Projectile.ai[1]];

            if (npc != null && npc.active)
            {
                ChosenNPC = npc;
                time = 300;
            }

            TranscendenceUtils.NoExpertProjDamage(Projectile);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), ChosenNPC.Center, ChosenNPC.Center + Vector2.One.RotatedBy(rot) * 2000, 3 * Projectile.scale, ref reference))
                return true;
            else return false;
        }
        public override bool? CanDamage() => Projectile.timeLeft < (time - 60);
        public override void PostDraw(Color lightColor)
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Trail").Value;

            Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(rot) * 1000;

            sb.End();
            sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(20 * Projectile.scale), (int)(Center.Distance(pos) * 2)), null,
                Color.DeepSkyBlue, Projectile.DirectionTo(Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.Draw(sprite, new Rectangle(
                (int)(pos.X - Main.screenPosition.X), (int)(pos.Y - Main.screenPosition.Y), (int)(5 * Projectile.scale), (int)(Center.Distance(pos) * 1.75f)), null,
                Color.White, Projectile.DirectionTo(Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}