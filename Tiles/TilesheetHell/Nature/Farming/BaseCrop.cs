using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TranscendenceMod.Tiles.TilesheetHell.Nature.Farming
{
    public abstract class BaseCrop : ModTile
    {
        private const int fSX = 18;
        public float rotation;
        public abstract int GrowthDivider { get; }
        public abstract Color mapColor {  get; } 
        public enum CropAge : byte
        {
            Baby,
            Stem,
            Grass,
            Leaves,
            Grown
        }

        public override void SetStaticDefaults()
        {
            AddMapEntry(mapColor);

            DustType = DustID.Grass;
            HitSound = SoundID.Dig;
            MineResist = 1;

            Main.tileFrameImportant[Type] = true;
            TileID.Sets.IgnoredInHouseScore[Type] = true;
            TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
        }
        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            CropAge stage = GetAge(i, j);

            if (stage == CropAge.Grown) return;
            if (GrowthDivider > 0 && Main.rand.NextBool(GrowthDivider)) tile.TileFrameX += fSX;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile crop = Main.tile[i, j];

            int x = i - crop.TileFrameX / 18;
            int y = j - crop.TileFrameY / 18;
            Vector2 pos = new Vector2(x + 2 / 2, y).ToWorldCoordinates();

            rotation += MathHelper.ToRadians(1f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f)) * 0.5f;

            if (GetAge(i, j) == CropAge.Grown)
            {
                Main.EntitySpriteDraw(TextureAssets.Cursors[3].Value, pos + new Vector2(240, 176 + (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 4f)) - Main.screenPosition, null, Color.White, rotation, TextureAssets.Cursors[3].Value.Size() * 0.5f, 0.5f, SpriteEffects.None);
            }
            return base.PreDraw(i, j, spriteBatch);
        }
        public CropAge GetAge(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            return (CropAge)(tile.TileFrameX / fSX);
        }
    }
}
