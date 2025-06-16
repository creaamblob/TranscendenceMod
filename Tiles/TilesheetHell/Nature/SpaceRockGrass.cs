using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature
{
    public class SpaceRockGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<SpaceRock>()] = true;
            RegisterItemDrop(ModContent.ItemType<SpaceRockItem>());
            DustType = ModContent.DustType<SpaceRockDust>();
            AddMapEntry(new Color(80, 35, 255));
            HitSound = SoundID.Tink;
            MinPick = 55;
            MineResist = 4f;
        }
        public override void FloorVisuals(Player player)
        {
        }
    }
}
