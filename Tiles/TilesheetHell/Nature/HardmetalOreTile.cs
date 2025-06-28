using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class HardmetalOreTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[Type][TileID.Ebonstone] = true;
            Main.tileMerge[Type][TileID.Crimstone] = true;
            Main.tileMerge[Type][TileID.Pearlstone] = true;
            Main.tileMerge[Type][TileID.Dirt] = true;
            DustType = ModContent.DustType<HardmetalDust>();

            RegisterItemDrop(ModContent.ItemType<HardmetalOre>());
            AddMapEntry(new Color(79, 77, 126));
            HitSound = SoundID.Tink;
            MinPick = 55;
            MineResist = 2.75f;
        }
    }
}
