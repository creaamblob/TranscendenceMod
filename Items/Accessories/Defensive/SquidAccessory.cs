using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Accessories.Defensive
{
    public class SquidAccessory : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.width = 25;
            Item.height = 30;
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 10);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().BatteryAcc = 1;
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            var ankhshield = ModContent.ItemType<StormCloak>();
            return (incomingItem.type != ankhshield || equippedItem.type != ankhshield);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 7)
            .AddIngredient(ItemID.LogicSensor_Water, 2)
            .AddIngredient(ModContent.ItemType<Lightning>(), 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
