using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Projectiles.Equipment
{
    public class SuperBoom : ModProjectile
    {
        public float Rad;
        public float MaxRad;
        public float Alpha = 1;
        public int Tries;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1500;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;

            Projectile.friendly = true;
            Projectile.hostile = true;

            Projectile.timeLeft = 950;
            Projectile.extraUpdates = 52;
            Projectile.penetrate = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            MaxRad = Projectile.timeLeft;
            SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, Projectile.Center);

            int rad = 18 * 16;
            Player player = Main.player[Projectile.owner];
            int power = player.GetBestPickaxe() != null ? player.GetBestPickaxe().pick : 0;

            Vector2 center = Projectile.Center + new Vector2(Projectile.width * 0.25f, Projectile.height * 0.15f);

            if (power == 0)
            {
                int c = CombatText.NewText(Projectile.getRect(), Color.Red, Language.GetTextValue("Mods.TranscendenceMod.Messages.SuperBombFail"));
                Main.combatText[c].lifeTime = 120;
                return;
            }

            for (int i = -rad; i < rad; i++)
            {
                for (int j = -rad; j < rad; j++)
                {
                    Vector2 pos = center + new Vector2(i, j);
                    Tile tile = Framing.GetTileSafely((int)pos.X / 16, (int)pos.Y / 16);

                    if (Projectile.owner == Main.myPlayer && player != null && tile != null && (i * i + j * j) <= rad * rad)
                    {
                        if (tile.HasTile && player.HasEnoughPickPowerToHurtTile((int)(pos.X / 16), (int)(pos.Y / 16)))
                        {
                            WorldGen.KillTile((int)(pos.X / 16), (int)(pos.Y / 16));
                            //player.PickTile((int)(pos.X / 16), (int)(pos.Y / 16), power);
                        }
                    }
                }
            }
        }
        public override void AI()
        {
            Projectile.position = Projectile.Center;
            

            Projectile.width++; 
            Projectile.height++;
            Projectile.position.Y += 0.0625f;

            Projectile.position -= Projectile.Size * 0.5f;
            Alpha = MathHelper.Lerp(0, 2, (float)Projectile.timeLeft / (float)MaxRad);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Distance(Projectile.Center + new Vector2(Projectile.width * 0.25f, Projectile.height * 0.15f)) < (Projectile.width * 0.33f) && Projectile.timeLeft > 100)
                return true;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            string spritepath = "TranscendenceMod/Miscannellous/Assets/Perlin2";
            Texture2D sprite = ModContent.Request<Texture2D>(spritepath).Value;
            Color col = Color.Lerp(Color.Gold, Color.OrangeRed, (float)Math.Sin(Projectile.width / 32f));

            Vector2 scale = new Vector2(1.5f, 1f);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + Projectile.Size * scale * 0.66f;
            Rectangle drawArea = new Rectangle(0, 0, Projectile.width, Projectile.height);

            DrawData drawData = new DrawData(sprite, drawPosition, drawArea, col * Alpha, 0, Projectile.Size, scale, SpriteEffects.None);

            GameShaders.Misc["ForceField"].UseColor(col * Alpha);
            GameShaders.Misc["ForceField"].Apply(drawData);
            drawData.Draw(spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}