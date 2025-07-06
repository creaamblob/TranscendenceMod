using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.UI.Processer;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Tiles.BigTiles
{
    public class MaterialProcesser : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileSpelunker[Type] = true;
            Main.tileFrameImportant[Type] = true;

            //TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;

            DustType = ModContent.DustType<HardmetalDust>();
            AddMapEntry(new Color(57, 71, 95), CreateMapEntryName());

            HitSound = SoundID.Dig;
            MineResist = 2f;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.addTile(Type);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);

            //TranscendenceUtils.BigTileGlowmask(i, j, Texture + "_Glow", Vector2.Zero);
        }
        public override bool RightClick(int i, int j)
        {
            Tile altar = Main.tile[i, j];

            Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ProcesserTile = altar;
            Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ProcesserPos = new Vector2(i, j).ToWorldCoordinates();

            ProcesserUIDrawing.Visible = true;

            return base.RightClick(i, j);
        }
        public override bool CanExplode(int i, int j) => false;
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            //player.cursorItemIconEnabled = true;

            //player.cursorItemIconID = ItemID.GoldCoin;
        }
    }
}
