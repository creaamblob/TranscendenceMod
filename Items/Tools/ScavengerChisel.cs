using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Farming.Seeds;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Weapons.Ranged;

namespace TranscendenceMod.Items.Tools
{
    public class ScavengerChisel : ModItem
    {
        public bool Tier2;
        public int Uses;
        public int MaxUses = 25;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((float)(MaxUses - Uses));
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.width = 18;
            Item.height = 18;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(gold: 5);
            Item.autoReuse = true;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 pos = Main.MouseWorld;
            pos /= 16;
            Tile tile = Main.tile[(int)pos.X, (int)pos.Y];
            if ((tile.TileType == TileID.Silt || tile.TileType == TileID.Slush || tile.TileType == TileID.DesertFossil) && player.Distance(Main.MouseWorld) < (4 * 16))
            {
                int CurrentItem = ModContent.ItemType<ThrowingPebble>();
                switch (Main.rand.Next(0, 15))
                {
                    case 0: CurrentItem = ModContent.ItemType<CaveWood>(); break;
                    case 1: CurrentItem = ItemID.Ruby; break;
                    case 2: CurrentItem = ItemID.Amber; break;
                    case 3: CurrentItem = ItemID.Topaz; break;
                    case 4: CurrentItem = ItemID.Emerald; break;
                    case 6: CurrentItem = ItemID.Diamond; break;
                    case 7: CurrentItem = ItemID.Sapphire; break;
                    case 8: CurrentItem = ItemID.Acorn; break;
                    case 9: CurrentItem = ItemID.GrassSeeds; break;
                    case 10: CurrentItem = ItemID.Cobweb; break;
                    case 11: CurrentItem = WorldGen.crimson ? ItemID.CorruptSeeds : ItemID.CrimsonSeeds; break;
                    case 12: CurrentItem = ModContent.ItemType<CocoaBeanSeeds>(); break;
                    case 13: CurrentItem = ItemID.Fertilizer; break;
                    case 14: CurrentItem = ModContent.ItemType<CarbonOre>(); break;
                }
                Item.NewItem(player.GetSource_ItemUse(Item), new Rectangle((int)pos.X * 16, (int)pos.Y * 16, 1, 1), CurrentItem, Main.rand.Next(1, 3));
                WorldGen.KillTile((int)pos.X, (int)pos.Y, false, false, true);


                if (++Uses >= MaxUses)
                {
                    SoundEngine.PlaySound(SoundID.Item37);
                    Item.TurnToAir();
                }
            }
            return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (Uses < 1)
                return;

            Texture2D sprite = ModContent.Request<Texture2D>($"Terraria/Images/UI/GolfSwingBarPanel").Value;
            Texture2D sprite2 = ModContent.Request<Texture2D>($"Terraria/Images/UI/GolfSwingBarFill").Value;
            spriteBatch.Draw(sprite, new Rectangle((int)(position.X - 13.5f), (int)position.Y + 14, 27, 14), Color.White);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)position.X - 11, (int)position.Y + 18, (int)MathHelper.Lerp(23f, 0f, (float)Uses / (float)MaxUses), 6), Color.Blue);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(nameof(ItemID.SilverBar), 3)
            .AddRecipeGroup(nameof(ItemID.CopperBar), 4)
            .AddIngredient(ItemID.Gel, 2)
            .AddTile(TileID.WorkBenches)
            .DisableDecraft()
            .Register();
        }
    }
}
