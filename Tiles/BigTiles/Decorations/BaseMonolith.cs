using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TranscendenceMod.Tiles.BigTiles.Decorations
{
    public abstract class BaseMonolith : ModTile
    {
        public abstract int DropItem { get; }
        public abstract int BreakDust { get; }
        public abstract Color mapCol { get; }
        public abstract void Effects(Player player);
        public abstract string ActiveIcon { get; }
        public abstract string InActiveIcon { get; }
        public abstract bool UseIcon { get; }

        public override void SetStaticDefaults()
        {
            RegisterItemDrop(DropItem);
            TileID.Sets.HasOutlines[Type] = true;

            Main.tileFrameImportant[Type] = true;

            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.addTile(Type);

            DustType = BreakDust;
            AddMapEntry(mapCol);

            HitSound = SoundID.Dig;

            AnimationFrameHeight = 54;
            AdjTiles = new int[] { TileID.LunarMonolith };
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.LunarMonolith];
            frameCounter = Main.tileFrameCounter[TileID.LunarMonolith];
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            Player player = Main.LocalPlayer;
            if (player is null)
                return;

            if (player.active && IsActive(i, j) && closer)
            {
                Effects(player);
            }
        }

        public bool IsActive(int i, int j)
        {
            return Main.tile[i, j].TileFrameY >= 54;
        }
        public void Activate(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 16 % 3;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 3;
            for (int l = x; l < x + 2; l++)
            {
                for (int m = y; m < y + 3; m++)
                {
                    if (Main.tile[l, m].TileType == Type)
                    {
                        if (Main.tile[l, m].TileFrameY < 54)
                        {
                            Main.tile[l, m].TileFrameY += 54;
                        }
                        else
                        {
                            Main.tile[l, m].TileFrameY -= 54;
                        }
                    }
                }
            }
        }
        public override void HitWire(int i, int j)
        {
            Activate(i, j);
        }
        public override bool RightClick(int i, int j)
        {
            Activate(i, j);
            SoundEngine.PlaySound(SoundID.Mech);
            return base.RightClick(i, j);
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = DropItem;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile altar = Main.tile[i, j];
            const float TwoPi = (float)Math.PI * 2f;

            int x = i - altar.TileFrameX / 18;
            int y = j - altar.TileFrameY / 18;
            int height = altar.TileFrameY % AnimationFrameHeight == 36 ? 18 : 16;
            int frameYOffset = Main.tileFrame[Type] * AnimationFrameHeight;

            Vector2 offScreenRange = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 pos = new Vector2(x + 2 / 2, y).ToWorldCoordinates();
            Vector2 pos2 = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + offScreenRange;

            Rectangle rec = new Rectangle(altar.TileFrameX, IsActive(i, j) ? altar.TileFrameY + frameYOffset : altar.TileFrameY, 16, height);

            Texture2D sprite2 = ModContent.Request<Texture2D>($"{Texture}").Value;

            if (UseIcon)
            {
                Texture2D eyeC = ModContent.Request<Texture2D>(Texture + InActiveIcon).Value;
                Texture2D eyeA = ModContent.Request<Texture2D>(Texture + ActiveIcon).Value;

                Texture2D eye = IsActive(i, j) ? eyeA : eyeC;

                float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 3f);
                offset *= IsActive(i, j) ? 4 : 0;

                Main.EntitySpriteDraw(eye, pos - new Vector2(-180, (IsActive(i, j) ? -232.5f : -182.5f) - offset) - Main.screenPosition, null, Color.White, 0, eye.Size() * 0.5f, 1, SpriteEffects.None);
            }

            Main.EntitySpriteDraw(sprite2, pos2 + new Vector2(20, 245), rec, Lighting.GetColor(i, j), 0, sprite2.Size() * 0.5f, 1, SpriteEffects.None);
            return false;
        }
    }
}
