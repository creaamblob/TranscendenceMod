using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Weapons.Magic;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons.Magic
{
    public class ConstellationsProj : ModProjectile
    {
        public Projectile proj;
        public float Rotation;
        public bool Dying;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 26;

            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 80;
            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic; 

            Projectile.friendly = true;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1500;

            Projectile.light = 0.75f * Projectile.scale;
            if (Projectile.timeLeft < 40 && Projectile.scale > 0)
                Projectile.scale -= 0.025f;

            if (player.altFunctionUse == 2 || player.HeldItem.type != ModContent.ItemType<Constellations>())
                Dying = true;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.ai[1] == Projectile.ai[1] + 1 && projectile != null && projectile.active && projectile.Center.Distance(Projectile.Center) < 3500 && projectile.type == Type)
                {
                    proj = projectile;
                }
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            TranscendenceUtils.DustRing(Projectile.Center, 6, ModContent.DustType<ArenaDust>(), Color.White, 0.5f, 2f, 4f);
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].Distance(Projectile.Center) < 1500 && Projectile.ai[1] == 0)
                {
                    Main.player[Projectile.owner] = Main.player[i];
                    Main.player[Projectile.owner].GetModPlayer<TranscendencePlayer>().ConstellationsIndex++;
                    Projectile.ai[1] = Main.player[Projectile.owner].GetModPlayer<TranscendencePlayer>().ConstellationsIndex;
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (proj != null && proj.active)
            {
                bool boolean = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                    proj.Center, 15, ref reference);
                if (boolean)
                {
                    return true;
                }
                else return projHitbox.Intersects(targetHitbox);
            }
            else return projHitbox.Intersects(targetHitbox);
        }
        public override void PostDraw(Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/BloomLine2").Value;

            if (proj != null && proj.active)
            {
                sb.End();
                sb.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                sb.Draw(sprite, new Rectangle(
                    (int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), (int)(75 * Projectile.scale),
                    (int)(Projectile.Distance(proj.Center) * 2.04f)), null,
                    Color.DeepSkyBlue * 0.5f, Projectile.DirectionTo(proj.Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);

                sb.Draw(sprite, new Rectangle(
                    (int)(Projectile.Center.X - Main.screenPosition.X), (int)(Projectile.Center.Y - Main.screenPosition.Y), (int)(25 * Projectile.scale),
                    (int)(Projectile.Distance(proj.Center) * 2.04f)), null,
                    Color.White, Projectile.DirectionTo(proj.Center).ToRotation() + MathHelper.PiOver2, sprite.Size() * 0.5f, SpriteEffects.None, 0);


                sb.End();
                sb.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
            TranscendenceUtils.DrawEntity(Projectile, Color.White, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, null);
        }
    }
}