using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class CarbonOreTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[Type][TileID.Ebonstone] = true;
            Main.tileMerge[Type][TileID.Crimstone] = true;
            Main.tileMerge[Type][TileID.Pearlstone] = true;
            Main.tileMerge[Type][TileID.Sandstone] = true;
            Main.tileMerge[Type][TileID.HardenedSand] = true;
            Main.tileMerge[Type][TileID.CorruptSandstone] = true;
            Main.tileMerge[Type][TileID.CrimsonSandstone] = true;
            Main.tileMerge[Type][TileID.HallowSandstone] = true;
            Main.tileMerge[Type][TileID.HardenedSand] = true;
            Main.tileMerge[Type][TileID.CorruptHardenedSand] = true;
            Main.tileMerge[Type][TileID.CrimsonHardenedSand] = true;
            Main.tileMerge[Type][TileID.HallowHardenedSand] = true;
            Main.tileMerge[Type][ModContent.TileType<VolcanicStone>()] = true;
            Main.tileMerge[Type][TileID.Ash] = true;
            DustType = ModContent.DustType<CarbonDust>();

            RegisterItemDrop(ModContent.ItemType<CarbonOre>());
            AddMapEntry(new Color(18, 19, 53));
            HitSound = SoundID.Tink;
            MinPick = 35;
            MineResist = 3.5f;
        }
    }
}
