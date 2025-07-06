using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TranscendenceMod.Tiles
{
    public class FourthWallBrick : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;

            DustType = DustID.Clay;
            HitSound = SoundID.Dig;

            AddMapEntry(new Color(113, 44, 12));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
        }
    }
}
