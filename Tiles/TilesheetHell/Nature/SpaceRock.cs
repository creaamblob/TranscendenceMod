using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class SpaceRock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<SpaceRockGrass>()] = true;
            DustType = ModContent.DustType<SpaceRockDust>();
            AddMapEntry(new Color(160, 90, 255));
            HitSound = SoundID.Tink;
            MinPick = 55;
            MineResist = 4f;
        }
        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j - 1].HasTile && (
                Main.tile[i - 1, j].TileType == ModContent.TileType<SpaceRockGrass>() ||
                Main.tile[i + 1, j].TileType == ModContent.TileType<SpaceRockGrass>() ||
                Main.tile[i + 1, j + 1].TileType == ModContent.TileType<SpaceRockGrass>() ||
                Main.tile[i - 1, j + 1].TileType == ModContent.TileType<SpaceRockGrass>() ||
                Main.tile[i + 1, j - 1].TileType == ModContent.TileType<SpaceRockGrass>() ||
                Main.tile[i - 1, j - 1].TileType == ModContent.TileType<SpaceRockGrass>()))
            {
                WorldUtils.Gen(new Point(i, j), new Shapes.Rectangle(1, 1), new Actions.SetTileKeepWall((ushort)ModContent.TileType<SpaceRockGrass>()));
                WorldGen.SquareTileFrame(i, j, true);
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
        }
    }
}
