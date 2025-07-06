using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Walls.Natural
{
    public class SunkenCataWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.DungeonBlue;
            AddMapEntry(new Color(80, 45, 128));
        }
    }
}
