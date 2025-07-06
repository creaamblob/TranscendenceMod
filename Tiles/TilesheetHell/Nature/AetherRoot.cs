using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class AetherRoot : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.CanBeClearedDuringGeneration[Type] = false;

            DustType = DustID.Corruption;
            AddMapEntry(new Color(181, 119, 255));
            HitSound = SoundID.Dig;
            MinPick = 35;
            MineResist = 3f;
        }
    }
}
