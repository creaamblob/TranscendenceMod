using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Tiles.TerrestrialSecond
{
    public class ExtraTerrestrialChair : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;

            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;

            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;

            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSatOnForNPCs[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;

            DustType = ModContent.DustType<ExtraTerrestrialDust>();
            AddMapEntry(new Color(165, 15, 175));

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);

            TileObjectData.addTile(Type);

            AdjTiles = new int[] { TileID.Chairs };
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
        }
        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            {
                player.sitting.SitDown(player, i, j);
                player.GamepadEnableGrappleCooldown();
            }
            return base.RightClick(i, j);
        }
        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            info.TargetDirection = -1;

            if (tile.TileFrameX != 0)
                info.TargetDirection = 1;

            info.AnchorTilePosition = new Point(i, j);    

            if (tile.TileFrameY % 40 == 0)
            {
                info.AnchorTilePosition.Y++;
            }

        }
        public override void MouseOver(int i, int j)
        {
            base.MouseOver(i, j);
        }
    }
}
