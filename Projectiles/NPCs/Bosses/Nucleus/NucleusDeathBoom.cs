using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Projectiles.NPCs.Bosses.Nucleus
{
    public class NucleusDeathBoom : ModProjectile
    {
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 7;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 26;

            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.light = 2.25f;
            Projectile.scale = 2;
        }
        public override void AI()
        {
            if (++Projectile.ai[2] > 2)
            {
                Projectile.frame++;
                Projectile.ai[2] = 0;
            }
        }
        public override void OnSpawn(IEntitySource source) => SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0, Volume = 0.5f }, Projectile.Center);
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;
            Vector2 pos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);

            int frameStuff = sprite.Height / Main.projFrames[Type];
            int y = frameStuff * Projectile.frame;
            Rectangle rec = new Rectangle(0, y, sprite.Width, frameStuff);

            Main.EntitySpriteDraw(sprite, pos, rec, Color.White, 0, rec.Size() * 0.5f, Projectile.scale, SpriteEffects.None);

            return false;
        }
    }
}