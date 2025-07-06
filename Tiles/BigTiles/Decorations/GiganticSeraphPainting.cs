using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles.BigTiles.Decorations
{
    public class GiganticSeraphPainting : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSpelunker[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
            MineResist = 100;
            MinPick = 1000;
            DustType = ModContent.DustType<NovaDust>();
            AddMapEntry(new Color(177, 0, 255));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16 };
            TileObjectData.addTile(Type);
        }
    }
}
