using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public class Soil : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            Main.tileMerge[Type][TileID.Grass] = true;
            Main.tileMerge[Type][TileID.Dirt] = true;
            Main.tileMerge[Type][TileID.Mud] = true;

            DustType = DustID.Mud;
            AddMapEntry(new Color(160, 110, 0));
            HitSound = SoundID.Dig;
        }
    }
}
