using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.GlobalStuff;

namespace TranscendenceMod.Items.Accessories.Shields
{
    public abstract class BaseShield : ModItem
    {
        /// <summary>
        /// 1 = NPCs only weak, 2 = NPCs only strong, 3 = NPCs and Projectiles, 4 = Muramasa Water only
        /// </summary>
        public abstract int ParryType { get; }
        public abstract int Leniency { get; }
        public abstract int Cooldown { get; }
        public abstract int DefenseAmount { get; }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.defense = DefenseAmount;
            Item.GetGlobalItem<TranscendenceItem>().ShieldParryLeniency = Leniency;
            Item.GetGlobalItem<TranscendenceItem>().ShieldParryCD = Cooldown;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.ModItem is BaseShield)
                return incomingItem.ModItem is not BaseShield;
            else return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().Parry = ParryType;
            player.GetModPlayer<TranscendencePlayer>().ShieldID = Item.type;
        }
    }
}
