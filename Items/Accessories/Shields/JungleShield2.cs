using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class JungleShield2 : BaseShield
    {
        public override int Leniency => 30;

        public override int Cooldown => 90;

        public override int DefenseAmount => 8;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
            Item.width = 29;
            Item.height = 38;
            Item.value = Item.buyPrice(gold: 7, silver: 25);
        }

        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().BeetleShield = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<JungleShield1>())
            .AddIngredient(ItemID.BeetleHusk, 12)
            .AddIngredient(ItemID.JungleSpores, 8)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
