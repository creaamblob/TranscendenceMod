using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Walls.Natural;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class VoidTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[Type][TileID.Ebonstone] = true;
            Main.tileMerge[Type][TileID.Crimstone] = true;
            Main.tileMerge[Type][TileID.Pearlstone] = true;
            Main.tileMerge[Type][TileID.Sandstone] = true;
            Main.tileMerge[Type][TileID.HardenedSand] = true;
            Main.tileMerge[Type][TileID.CorruptSandstone] = true;
            Main.tileMerge[Type][TileID.CrimsonSandstone] = true;
            Main.tileMerge[Type][TileID.HallowSandstone] = true;
            Main.tileMerge[Type][TileID.HardenedSand] = true;
            Main.tileMerge[Type][TileID.CorruptHardenedSand] = true;
            Main.tileMerge[Type][TileID.CrimsonHardenedSand] = true;
            Main.tileMerge[Type][TileID.HallowHardenedSand] = true;
            Main.tileMerge[Type][ModContent.TileType<VolcanicStone>()] = true;
            Main.tileMerge[Type][TileID.Ash] = true;
            DustType = ModContent.DustType<VoidDust>();

            AddMapEntry(new Color(18, 19, 53));
            HitSound = SoundID.Tink;
            MinPick = 35;
            MineResist = 3.5f;
        }
        public override void RandomUpdate(int i, int j)
        {
            int allowedAm = Main.maxTilesX * 2;
            bool noMaxTiles = TranscendenceWorld.VoidTilesCount < allowedAm;

            for (int k = -8; k < 8; k++)
            {
                for (int l = -8; l < 8; l++)
                {
                    int x = (int)(k + i);
                    int y = (int)(l + j);

                    if ((k * k + l * l) <= 8 * 8)
                    {
                        Tile tile = Main.tile[x, y];

                        if (VoidSeeds.VoidConsumable(tile) && tile.HasTile && noMaxTiles)
                        {
                            WorldUtils.Gen(new Point(x, y), new Shapes.Rectangle(1, 1), new Actions.SetTileKeepWall((ushort)ModContent.TileType<VoidTile>()));
                            WorldGen.SquareTileFrame(x, y, true);
                            NetMessage.SendTileSquare(-1, x, y, 1);

                        }

                        if (tile.WallType != ModContent.WallType<VoidWall>() && tile.WallType != WallID.None && noMaxTiles)
                        {

                            WorldUtils.ClearWall(x, y);
                            WorldUtils.Gen(new Point(x, y), new Shapes.Rectangle(1, 1), new Actions.PlaceWall((ushort)ModContent.WallType<VoidWall>()));

                            WorldGen.SquareTileFrame(x, y, true);
                            NetMessage.SendTileSquare(-1, x, y, 1);

                        }
                    }
                }
            }
        }
    }
}
