using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Consumables.Boss;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.NPCs.Passive;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.Tiles.BigTiles
{
    public class ShimmerAltar : ModTile
    {
        public bool PlacedSacrifice;
        public bool SummoningActive;


        public int artifact = ModContent.ItemType<CosmicArtifact>();

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;

            DustType = ModContent.DustType<NovaDust>();
            AddMapEntry(new Color(255, 255, 255), CreateMapEntryName());

            HitSound = SoundID.Dig;
            MineResist = 100;
            MinPick = 1000;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.addTile(Type);
        }
        public const int FrameDims = 18 * 3;
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (drawData.tileFrameX % FrameDims == 0 && drawData.tileFrameY % FrameDims == 0)
            {
                Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
            }
        }
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile altar = Main.tile[i, j];

            SummoningActive = TranscendenceWorld.AnyProjectiles(ModContent.ProjectileType<CelestialSeraphSummoner>());

            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
                offScreen = Vector2.Zero;

            Point p = new Point(i, j);
            Tile tile = Main.tile[p.X, p.Y];

            if (tile == null || !tile.HasTile)
                return;

            Vector2 worldPos = p.ToWorldCoordinates(24f, 64f);
            Vector2 drawPos = worldPos + offScreen - Main.screenPosition - new Vector2(0f, 90f + (float)Math.Sin(TranscendenceWorld.UniversalRotation * 2f) * 4f);

            Texture2D art = ModContent.Request<Texture2D>("TranscendenceMod/Items/Consumables/Boss/CosmicArtifact").Value;

            if (PlacedSacrifice)
                Main.EntitySpriteDraw(art, drawPos, null, Color.White, 0, art.Size() * 0.5f, 1f, SpriteEffects.None);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);

            TranscendenceUtils.BigTileGlowmask(i, j, Texture, Vector2.Zero);
        }
        public override bool RightClick(int i, int j)
        {
            Tile altar = Main.tile[i, j];
            Player local = Main.LocalPlayer;

            int x = i - altar.TileFrameX / 18;
            int y = j - altar.TileFrameY / 18;
            Vector2 pos = new Vector2(x + 2 / 2, y).ToWorldCoordinates();


            if (PlacedSacrifice)
            {
                Projectile.NewProjectile(local.GetSource_TileInteraction(i, j), pos - new Vector2(0, 16), Vector2.Zero, ModContent.ProjectileType<CelestialSeraphSummoner>(), 0, 0, local.whoAmI);

                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.type == ModContent.NPCType<LateGameNPC>() && npc.active && npc != null)
                    {
                        EmoteBubble.NewBubble(EmoteID.EmotionAlert, new WorldUIAnchor(npc), 120);
                    }
                }

                PlacedSacrifice = false;
                return true;
            }

            if (!PlacedSacrifice && local.HeldItem.type == artifact)
                PlacedSacrifice = true;

            return base.RightClick(i, j);
        }
        public override bool CanExplode(int i, int j) => false;
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;

            player.cursorItemIconID = !PlacedSacrifice ? ModContent.ItemType<CosmicArtifact>() :
                TranscendenceWorld.EncouteredSeraph ? ModContent.ItemType<SeraphHeadRevealed>() :
                ModContent.ItemType<SeraphHeadBlackOut>();
        }
    }
}
