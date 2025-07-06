using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TranscendenceMod.Tiles.BigTiles.Decorations
{
    public class FaeTrophy : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            DustType = DustID.WoodFurniture;
            AddMapEntry(new Color(137, 0, 255));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.addTile(Type);
        }
    }
}
