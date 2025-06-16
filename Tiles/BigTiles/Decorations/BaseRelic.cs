using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Items.Consumables.Placeables;

namespace TranscendenceMod.Tiles.BigTiles.Decorations
{
    public abstract class BaseRelic : ModTile
    {
        public const int FrameWidth = 18 * 3;
        public const int FrameHeight = 18 * 4;

        public Asset<Texture2D> RelicIconSprite;
        public virtual string RelicIconPath => "TranscendenceMod/Items/Weapons/Magic/PillarsOfCreation";
        public override string Texture => "TranscendenceMod/Tiles/BigTiles/Decorations/BaseRelic";
        public abstract int HeldItem
        {
            get;
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                RelicIconSprite = ModContent.Request<Texture2D>(RelicIconPath);
            }
        }
        public override void Unload()
        {
            RelicIconSprite = null;
        }

        public override void SetStaticDefaults()
        {
            RegisterItemDrop(HeldItem);

            Main.tileShine[Type] = 400;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleHorizontal = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.LavaDeath = false;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);

            TileObjectData.addTile(Type);
            AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Relic"));
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (drawData.tileFrameX % FrameWidth == 0 && drawData.tileFrameY % FrameHeight == 0)
            {
                Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
            }
        }

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
                offScreen = Vector2.Zero;

            Point p = new Point(i, j);
            Tile tile = Main.tile[p.X, p.Y];
            if (tile == null || !tile.HasTile)
                return;

            Texture2D texture = RelicIconSprite.Value;
            int frameY = tile.TileFrameX / FrameWidth;

            Rectangle frame = texture.Frame(1, 1, 0, frameY);
            Vector2 origin = frame.Size() / 2f;

            bool dir = tile.TileFrameY / FrameHeight != 0;
            SpriteEffects effects = dir ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 5f);
            Vector2 worldPos = p.ToWorldCoordinates(24f, 64f);
            Vector2 drawPos = worldPos + offScreen - Main.screenPosition - new Vector2(0f, 40f) + new Vector2(0f, offset * 4f);

            Color color = Lighting.GetColor(p.X, p.Y);
            //Draw the relic icon
            spriteBatch.Draw(texture, drawPos, frame, color, 0f, origin, 1f, effects, 0f);

            //Draw the gold aura
            float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 2f) * 0.3f + 0.7f;
            Color effectColor = color;
            effectColor.A = 0;
            effectColor = effectColor * 0.1f * scale;
            float offset2 = 6f + offset * 2f;
            float oneOutOfSix = 1f / 6f;

            for (float k = 0f; k < 1f; k += oneOutOfSix)
                spriteBatch.Draw(texture, drawPos + (MathHelper.TwoPi * k).ToRotationVector2() * offset2, frame, effectColor, 0f, origin, 1f, effects, 0f);
        }
    }

    public class SeraphRelic : BaseRelic
    {
        public override string RelicIconPath => "TranscendenceMod/Tiles/BigTiles/Decorations/Relics/Seraph";
        public override string Texture => "TranscendenceMod/Tiles/BigTiles/Decorations/BaseRelic";
        public override int HeldItem => ModContent.ItemType<SeraphRelicItem>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
    public class SerpentRelic : BaseRelic
    {
        public override string RelicIconPath => "TranscendenceMod/Tiles/BigTiles/Decorations/Relics/FrostSerpent";
        public override string Texture => "TranscendenceMod/Tiles/BigTiles/Decorations/BaseRelic";
        public override int HeldItem => ModContent.ItemType<SerpentRelicItem>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
    public class NucleusRelic : BaseRelic
    {
        public override string RelicIconPath => "TranscendenceMod/Tiles/BigTiles/Decorations/Relics/Nucleus";
        public override string Texture => "TranscendenceMod/Tiles/BigTiles/Decorations/BaseRelic";
        public override int HeldItem => ModContent.ItemType<NucleusRelicItem>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
}
