using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Buffs;

namespace TranscendenceMod.Tiles.BigTiles
{
    public class LifeFruitArenaTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileID.Sets.FramesOnKillWall[Type] = true;
              
            DustType = DustID.JungleSpore;
            AddMapEntry(new Color(99, 151, 31), CreateMapEntryName());
            HitSound = SoundID.Dig;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.addTile(Type);
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!(Main.LocalPlayer.dead && Main.LocalPlayer.active)) Main.LocalPlayer.AddBuff(ModContent.BuffType<LifeFruitArenaBuff>(), 6);
        }
    }
}
