using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables.TerrestrialItems;

namespace TranscendenceMod.Tiles.TerrestrialSecond
{
    public class ExtraTerrestrialPlatform : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileTable[Type] = true;
            TileID.Sets.Platforms[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            RegisterItemDrop(ModContent.ItemType<ExtraTerrestrialPlatformItem>());

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
            AdjTiles = new int[] { TileID.Platforms };

            DustType = ModContent.DustType<ExtraTerrestrialDust>();
            AddMapEntry(new Color(165, 15, 175));
            HitSound = SoundID.Dig;
            MineResist = 2;
            MinPick = 0;

            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinatePadding = 2;

            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleMultiplier = 27;
            TileObjectData.newTile.StyleWrapLimit = 27;

            TileObjectData.newTile.UsesCustomCanPlace = false;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);

        }
    }
}
