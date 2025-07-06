using Terraria;
using Terraria.ID;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public class GiantShellPurple : BaseShield
    {
        public override int Leniency => 45;
        
        public override int Cooldown => 90;

        public override int DefenseAmount => 2;
        
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 3, silver: 75);
        }
        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
            player.GetModPlayer<TranscendencePlayer>().PurpleShell = true;
        }
    }
}
