using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Consumables.Boss
{
    public class HeavyBoulder : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 9999;

            Item.rare = ItemRarityID.Blue;
            Item.consumable = true;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 22;
            Item.useAnimation = 22;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                TranscendenceWorld.IntiateBoulderRain();
                player.itemTime = 0;
                player.itemAnimation = 0;
            }
            return !TranscendenceUtils.BossAlive() && !TranscendenceWorld.BoulderRain;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.StoneBlock, 50)
            .AddIngredient(ItemID.Boulder, 5)
            .AddTile(TileID.HeavyWorkBench)
            .Register();
        }
    }
}
