using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Tools.Compasses
{
    public class CorruptCompass : ModItem
    {
        public Vector2 Pos;
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.width = 18;
            Item.height = 18;

            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item92;
            Item.holdStyle = ItemHoldStyleID.HoldLamp;

            Item.value = Item.buyPrice(gold: 15);
            Item.autoReuse = true;

            Item.shoot = ProjectileID.PhantasmalSphere;
            Item.shootSpeed = 10;
        }
        public override void HoldItem(Player player)
        {
            if (Pos != Vector2.Zero && player.ItemTimeIsZero)
                Dust.QuickDustLine(player.Center + new Vector2(22 * player.direction, 4), player.Center + new Vector2(22 * player.direction, 4) + Vector2.One.RotatedBy(player.DirectionTo(Pos).ToRotation() - MathHelper.PiOver4) * 5, 50, Color.White);
            base.HoldItem(player);
        }
        public bool corruptTile(Tile tile)
        {
            return tile.TileType == TileID.CorruptGrass || tile.TileType == TileID.Ebonsand || tile.TileType == TileID.Ebonstone || tile.TileType == TileID.CorruptIce || tile.TileType == TileID.CorruptVines ||
                tile.TileType == TileID.CorruptHardenedSand || tile.TileType == TileID.CorruptJungleGrass || tile.TileType == TileID.CorruptSandstone || tile.TileType == TileID.CorruptThorns || tile.TileType == TileID.CorruptPlants;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int x = (int)player.position.X / 16;
            int xDest = player.direction > 0 ? Main.maxTilesX - 55 : 55;

            if (player.direction == 1)
            {
                for (int i = x; i < xDest; i++)
                {
                    for (int j = 55; j < (Main.maxTilesY - 55); j++)
                    {
                        Tile tile = Main.tile[i, j];

                        if (tile.HasTile && corruptTile(tile))
                        {
                            Pos = new Vector2(i, j) * 16;
                        }
                    }
                }
            }
            else
            {
                for (int i = x; i > xDest; i--)
                {
                    for (int j = 55; j < (Main.maxTilesY - 55); j++)
                    {
                        Tile tile = Main.tile[i, j];

                        if (tile.HasTile && corruptTile(tile))
                        {
                            Pos = new Vector2(i, j) * 16;
                        }
                    }
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Compass>())
            .AddIngredient(ItemID.EbonstoneBlock, 50)
            .AddIngredient(ItemID.DemoniteBar, 14)
            .AddIngredient(ItemID.CorruptSeeds, 7)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
