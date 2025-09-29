using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Tools.Compasses
{
    public class SolarPoweredCompass : Compass
    {
        public int Amount = 0;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int x = (int)player.position.X / 16;
            int y = (int)player.position.Y / 16;
            int xDest = player.direction > 0 ? Main.maxTilesX - 55 : 55;
            bool found = false;

            if (player.direction == 1)
            {
                for (int i = x; i < xDest; i++)
                {
                    if (found)
                        break;

                    for (int j = 55; j < (Main.maxTilesY - 55); j++)
                    {
                        Tile tile = Main.tile[i, j];

                        if (tile.HasTile && tile.TileType == TileID.LihzahrdBrick && tile.WallType == WallID.LihzahrdBrickUnsafe)
                        {
                            Amount++;
                            if (Amount >= 70)
                            {
                                Pos = new Vector2(i, j) * 16;
                                found = true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = x; i > xDest; i--)
                {
                    if (found)
                        break;

                    for (int j = 55; j < (Main.maxTilesY - 55); j++)
                    {
                        Tile tile = Main.tile[i, j];

                        if (tile.HasTile && tile.TileType == TileID.LihzahrdBrick && tile.WallType == WallID.LihzahrdBrickUnsafe)
                        {
                            Amount++;
                            if (Amount >= 70)
                            {
                                Pos = new Vector2(i, j) * 16;
                                found = true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteBar, 12)
            .AddIngredient(ItemID.RichMahogany, 25)
            .AddIngredient(ItemID.JungleSpores, 12)
            .AddIngredient(ItemID.SlushBlock, 7)
            .AddIngredient(ItemID.Emerald, 3)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
