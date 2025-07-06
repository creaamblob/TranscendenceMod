using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class VolcanicStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = DustID.Stone;

            RegisterItemDrop(ItemID.StoneBlock);
            AddMapEntry(new Color(120, 82, 82));
            HitSound = SoundID.Tink;
            MineResist = 3f;
            Main.tileMerge[Type][TileID.Ash] = true;
            Main.tileMerge[Type][TileID.Hellstone] = true;
            Main.tileMerge[Type][TileID.Ruby] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[Type][TileID.Ebonstone] = true;
            Main.tileMerge[Type][TileID.Crimstone] = true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.rand.NextBool(90) && !Main.tile[i, j - 1].HasTile)
            {
                Dust f = Dust.NewDustPerfect(new Vector2(i * 16, j * 16), ModContent.DustType<Ember>(), new Vector2(-1.25f, -5), 0, default, 0.4f);
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Main.tileNoSunLight[Type] = false;
            TileID.Sets.DrawsWalls[Type] = true;

            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
                offScreen = Vector2.Zero;

            Tile safe = Framing.GetTileSafely(i, j);
            int frameSizeY = safe.TileFrameY == 36 ? 18 : 16;

            int x = (int)((i * 16) - Main.screenPosition.X);
            int y = (int)((j * 16) - Main.screenPosition.Y);

            float br = Lighting.Brightness(i, j);
            Color col = Color.Lerp(new Color(1f * br, 0.4f * br, 0.4f * br), new Color(0.8f * br, 0.6f * br, 0.4f * br), (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2));
            Vector2 pos = new Vector2(x, y) + offScreen;
            if (safe.BlockType == BlockType.Solid)
            {
                Main.spriteBatch.Draw(sprite, pos, new Rectangle(safe.TileFrameX, safe.TileFrameY, 16, frameSizeY), col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(sprite, pos + new Vector2(0, 8), new Rectangle(safe.TileFrameX, safe.TileFrameY, 16, 8), col, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void FloorVisuals(Player player)
        {
            //player.AddBuff(BuffID.OnFire3, 120);
            //player.AddBuff(BuffID.Burning, 60);
        }
    }
}
