using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class LihzardianBulwark : BaseShield
    {
        public override int Leniency => 35;

        public override int Cooldown => 90;

        public override int DefenseAmount => 12;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
            Item.width = 26;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 12, silver: 50);
        }

        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().LihzardianBulwarkEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<JungleShield1>())
            .AddIngredient(ItemID.LunarTabletFragment, 32)
            .AddIngredient(ItemID.RichMahogany, 20)
            .AddIngredient(ItemID.BeetleHusk, 4)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
