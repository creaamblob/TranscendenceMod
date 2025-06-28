using Terraria;
using Terraria.ID;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class MoltenShield : BaseShield
    {
        public override int Leniency => 40;

        public override int Cooldown => 90;

        public override int DefenseAmount => 3;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 5);
        }
        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().MoltenShieldEquipped = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.HellstoneBar, 14)
            .AddIngredient(ItemID.Fireblossom, 6)
            .AddTile(TileID.Hellforge)
            .Register();
        }
    }
}