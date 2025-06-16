using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles.TerrestrialSecond
{
    public class ExtraTerrestrialCandle : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileNoAttach[Type] = true;

            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;

            DustType = ModContent.DustType<ExtraTerrestrialDust>();
            AddMapEntry(new Color(165, 15, 175));
            AnimationFrameHeight = 24;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return true;
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            Main.tile[i, j].TileFrameY = 0;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile candle = Main.tile[i, j];

            int x = i - candle.TileFrameX / 18;
            int y = j - candle.TileFrameY / 18;

            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
                offScreen = Vector2.Zero;

            Vector2 pos = new Vector2(x + 2 / 2, y).ToWorldCoordinates() - new Vector2(16, 8) + offScreen;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, null);
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}_Flame").Value;

            if (candle.TileFrameY == 0)
            {
                Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), TranscendenceWorld.CosmicPurple.R / 255f, TranscendenceWorld.CosmicPurple.G / 255f, TranscendenceWorld.CosmicPurple.B / 255f);
                Main.EntitySpriteDraw(sprite, pos + new Vector2(Main.rand.Next(-1, 2) * 4, Main.rand.Next(-2, 2) * 4) - Main.screenPosition, null, Color.Violet * 0.75f, 0, sprite.Size() * 0.5f, 1, SpriteEffects.None);
                Main.EntitySpriteDraw(sprite, pos + new Vector2(Main.rand.Next(-1, 2) * 4, Main.rand.Next(-2, 2) * 4) - Main.screenPosition, null, Color.Violet * 0.75f, 0, sprite.Size() * 0.5f, 1, SpriteEffects.None);
                Main.EntitySpriteDraw(sprite, pos - Main.screenPosition, null, Color.White, 0, sprite.Size() * 0.5f, 1, SpriteEffects.None);
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null);

            if (Main.tile[i, j].TileFrameY == 0) Main.tileSolidTop[Main.tile[i, j].TileType] = true;
            else Main.tileSolidTop[Main.tile[i, j].TileType] = false;
            base.PostDraw(i, j, spriteBatch);
        }
        public override bool RightClick(int i, int j)
        {
            SoundEngine.PlaySound(SoundID.Mech);
            if (Main.tile[i, j].TileFrameY == 0)
                Main.tile[i, j].TileFrameY = 24;
            else
                Main.tile[i, j].TileFrameY = 0;
            return base.RightClick(i, j);
        }
    }
}
