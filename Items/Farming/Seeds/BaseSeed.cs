using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Farming.Seeds
{
    public abstract class BaseSeed : ModItem
    {
        public virtual bool allowed(Tile tile2) => false;
        public abstract int Tile { get; }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 15;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.maxStack = 9999;

            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.consumable = true;
            Item.autoReuse = true;

            Item.width = 14;
            Item.height = 20;
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 pos = Main.MouseWorld;
            pos /= 16;

            Tile tile = Main.tile[(int)pos.X, (int)pos.Y];
            Tile tile2 = Main.tile[(int)pos.X, (int)pos.Y + 1];

            if (!tile.HasTile && allowed(tile2) && player.Distance(Main.MouseWorld) < (4 * 16))
            {
                WorldGen.PlaceTile((int)pos.X, (int)pos.Y, Tile);

                //Custom consumption code
                if (Item.stack > 1)
                    Item.stack -= 1;
                else Item.TurnToAir();
            }

            return false;
        }
    }
}
