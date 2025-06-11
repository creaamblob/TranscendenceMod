using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Pets
{
    public class SeraphPet : ModProjectile
    {
        public bool Faraway;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
            Main.projPet[Type] = true;

            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;

            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 4, 5)
                .WithOffset(-25f, -10f)
                .WithCode(DelegateMethods.CharacterPreview.Float);
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 45;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 3;
            Projectile.penetrate = -1;

            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 4, 5)
    .WithOffset(-12f, -40f)
    .WithCode(DelegateMethods.CharacterPreview.Float);
        }

        public override void PostDraw(Color lightColor)
        {
            //Check if it isn't a preview dummy (title screen pet) for title screen compatibility
            if (!Projectile.isAPreviewDummy)
            {
                TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, $"{Texture}_Glow", Projectile.rotation, Projectile.BottomRight, false, false, false,
                    Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

                TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, $"{Texture}_Glow2", Projectile.rotation, Vector2.Zero, true, false, true,
                    Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = Projectile.direction;
            Projectile.direction = player.Center.X > Projectile.Center.X ? 1 : -1;

            Movement();
            FrameStuff();

            if (player.Distance(Projectile.Center) > 500)
                Faraway = true;
            if (player.Distance(Projectile.Center) < 250) Faraway = false;

            if (!player.dead && player.HasBuff(ModContent.BuffType<SeraphPetBuff>()))
                Projectile.timeLeft = 3;

            void Movement()
            {
                if (++Projectile.ai[1] > 5 || Faraway)
                {
                    Vector2 targetVelocity = Projectile.DirectionTo((player.Center - new Vector2(55 * player.direction, Main.rand.Next(-60, 60))) + Vector2.One.RotatedByRandom(350) * Main.rand.Next(220, 350)) * (Faraway ? 20 : 10);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.05f);
                    Projectile.ai[1] = 0;
                }
            }

            void FrameStuff()
            {
                if (++Projectile.frameCounter > 5)
                {
                    Projectile.frame++;
                    if (Projectile.frame == 4 || Projectile.frame == 8)
                        Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                }
                if (Faraway && Projectile.frame < 4)
                    Projectile.frame = 5;
            }
        }
    }
}