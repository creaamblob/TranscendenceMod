using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles.TerrestrialSecond
{
    public class ExtraTerrestrialWorkBench : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            DustType = ModContent.DustType<ExtraTerrestrialDust>();
            AddMapEntry(new Color(165, 15, 175));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.addTile(Type);
            AdjTiles = new int[] { TileID.WorkBenches };
        }
    }
}
