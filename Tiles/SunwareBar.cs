using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TranscendenceMod.Tiles
{
    public class SunwareBar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;

            Main.tileShine[Type] = 750;
            Main.tileNoAttach[Type] = true;

            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;

            DustType = DustID.SolarFlare;
            AddMapEntry(new Color(215, 183, 26), Language.GetText("MabObject.MetalBar"));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
        }
    }
}
