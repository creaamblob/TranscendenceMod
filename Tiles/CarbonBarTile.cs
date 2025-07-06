using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles
{
    public class CarbonBarTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;

            Main.tileNoAttach[Type] = true;

            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;

            DustType = ModContent.DustType<CarbonDust>();
            AddMapEntry(new Color(18, 19, 53), Language.GetText("MabObject.MetalBar"));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
        }
    }
}
