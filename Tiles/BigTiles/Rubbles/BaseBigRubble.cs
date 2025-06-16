using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles.BigTiles.Rubbles
{
    public abstract class BaseBigRubble : ModTile
    {
        public virtual string spritePath => "TranscendenceMod/Tiles/BigTiles/Rubbles/BaseBigRubble";
        public virtual int dustType => ModContent.DustType<HardmetalDust>();
        public virtual Color mapColor => new Color(255, 255, 255);
        public override string Texture => spritePath;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileObsidianKill[Type] = true;

            DustType = dustType;
            AddMapEntry(mapColor);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
        }
    }
}
