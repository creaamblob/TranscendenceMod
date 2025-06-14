using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Tiles.BigTiles.Furniture
{
    public class StarcraftedOrgan : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;

            DustType = ModContent.DustType<StarcraftedDust>();
            AddMapEntry(new Color(90, 108, 154));

            HitSound = SoundID.Tink;
            MineResist = 10;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(1, -1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            TileObjectData.addTile(Type);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            TranscendenceUtils.BigTileGlowmask(i, j, Texture + "_Glow", Vector2.Zero);
        }
        public override bool CanExplode(int i, int j) => false;
    }
}
