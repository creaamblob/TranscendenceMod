using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public class StarSoil : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            Main.tileMerge[Type][TileID.Grass] = true;
            Main.tileMerge[Type][TileID.Dirt] = true;
            Main.tileMerge[Type][TileID.Mud] = true;
            Main.tileMerge[Type][ModContent.TileType<Soil>()] = true;

            DustType = DustID.Clay;
            AddMapEntry(new Color(121, 20, 69));
            HitSound = SoundID.Dig;
        }
    }
}
