using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TranscendenceMod.Tiles.BigTiles.Decorations
{
    public class NucleusTrophy : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            DustType = DustID.WoodFurniture;
            AddMapEntry(new Color(120, 85, 60));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.addTile(Type);
        }
    }
}
