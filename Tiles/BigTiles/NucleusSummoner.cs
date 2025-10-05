using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Nucleus;

namespace TranscendenceMod.Tiles.BigTiles
{
    public class NucleusSummoner : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileSpelunker[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;

            DustType = ModContent.DustType<NucleusBlood>();
            AddMapEntry(new Color(77, 98, 113), CreateMapEntryName());

            HitSound = SoundID.Dig;
            MineResist = 3f;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.addTile(Type);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);

            TranscendenceUtils.BigTileGlowmask(i, j, Texture + "_Glow", Vector2.Zero);


            Tile altar = Main.tile[i, j];

            Texture2D block = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/SeraphBoundary").Value;

            int x = i - altar.TileFrameX / 18;
            int y = j - altar.TileFrameY / 18;
            Vector2 pos = new Vector2(x + 2 / 2, y).ToWorldCoordinates() - new Vector2(0, 300 - 32);
            Vector2 pos2 = pos - new Vector2(47 * 16, 29 * 16 + 4);
            for (int l = 1; l < (58 * 2 + 2); l++)
            {
                spriteBatch.Draw(block, pos2 + new Vector2(l * 16, 0) - Main.screenPosition, null, Color.White * 0.025f, 0f, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(block, pos2 + new Vector2(l * 16, 31 * 32) - Main.screenPosition, null, Color.White * 0.025f, 0f, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            for (int m = 0; m < (31 * 2 + 1); m++)
            {
                spriteBatch.Draw(block, pos2 + new Vector2(0, m * 16) - Main.screenPosition, null, Color.White * 0.025f, 0f, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(block, pos2 + new Vector2(59 * 32, m * 16) - Main.screenPosition, null, Color.White * 0.025f, 0, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }

        }
        public override bool RightClick(int i, int j)
        {
            Tile altar = Main.tile[i, j];
            Player local = Main.LocalPlayer;

            int x = i - altar.TileFrameX / 18;
            int y = j - altar.TileFrameY / 18;

            Point point = new Point(x - 58, y - 31 * 2 + 4);
            Ref<int> solidTiles = new Ref<int>(0);

            WorldUtils.Gen(point, new Shapes.Rectangle(58 * 2 + 1, 31 * 2 - 1), Actions.Chain(new GenAction[]
            {
                new Actions.ContinueWrapper(Actions.Chain(new GenAction[]
                {
                    new Modifiers.IsSolid(),
                    new Actions.Custom((i, j, args) => {
                        Dust d = Dust.NewDustPerfect(new Vector2(i, j).ToWorldCoordinates(), DustID.TheDestroyer, Vector2.Zero);
                        d.noGravity = true;
                        return true; }),
                    new Actions.Scanner(solidTiles)
                }))
            }));


            Vector2 pos = new Vector2(x + 2 / 2, y).ToWorldCoordinates();
            int nucleus = ModContent.NPCType<ProjectNucleus>();

            if (!Main.dayTime && solidTiles.Value == 0 && !NPC.AnyNPCs(nucleus))
                NPC.NewNPC(local.GetSource_TileInteraction(i, j), (int)pos.X, (int)pos.Y - 300, nucleus);

            return base.RightClick(i, j);
        }
        public override bool CanExplode(int i, int j) => false;
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;

            player.cursorItemIconID = ModContent.ItemType<NucleusSummonerItem>();
        }
    }
}
