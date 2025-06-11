using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Walls.Natural
{
    public class VoidWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            DustType = ModContent.DustType<VoidDust>();
            AddMapEntry(new Color(18, 19, 53));
        }
    }
}
