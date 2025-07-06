using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class SpaceCrystal : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileShine[Type] = 40;
            Main.tileSpelunker[Type] = true;

            DustType = ModContent.DustType<SpaceCrystalDust>();
            AddMapEntry(new Color(96, 9, 108));
            HitSound = SoundID.Shatter;
            MinPick = 110;
            MineResist = 7f;
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
            float opacity = (float)(0.5f + Math.Sin(Main.GlobalTimeWrappedHourly * 3) * 0.175f);

            if (safe.BlockType == BlockType.Solid)
            {
                Main.spriteBatch.Draw(sprite, new Vector2(x, y) + offScreen, new Rectangle(safe.TileFrameX, safe.TileFrameY, 16, frameSizeY), Color.White * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                return false;
            }
            else return base.PreDraw(i, j, spriteBatch);
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Item item = player.GetBestPickaxe();

            if (item == null || item.pick < 110)
            {
                player.cursorItemIconText = "Pickaxe Power Too Low!";
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = Math.Sin(Main.GlobalTimeWrappedHourly * 4f) > 0 ? ItemID.PalladiumPickaxe : ItemID.CobaltPickaxe;
            }
        }
    }
}
