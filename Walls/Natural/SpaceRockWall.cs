using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Walls.Natural
{
    public class SpaceRockWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<ExtraTerrestrialDust>();
            AddMapEntry(new Color(80, 45, 128));
        }
    }
    public class SpaceRockWallUnsafe : ModWall
    {
        public override string Texture => "TranscendenceMod/Walls/Natural/SpaceRockWall";
        public override void SetStaticDefaults()
        {
            DustType = ModContent.DustType<ExtraTerrestrialDust>();
            AddMapEntry(new Color(80, 45, 128));
        }
    }
}
