using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Tiles.BigTiles.Furniture
{
    public class StarcraftedForge : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;

            DustType = ModContent.DustType<StarcraftedDust>();
            AddMapEntry(new Color(90, 108, 154));

            HitSound = SoundID.Tink;
            MineResist = 12;

            AdjTiles = new int[] { TileID.MythrilAnvil };
            AdjTiles = new int[] { TileID.AdamantiteForge };
            AdjTiles = new int[] { TileID.LunarCraftingStation };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.addTile(Type);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            TranscendenceUtils.BigTileGlowmask(i, j, Texture, Vector2.Zero);
        }
    }
}
