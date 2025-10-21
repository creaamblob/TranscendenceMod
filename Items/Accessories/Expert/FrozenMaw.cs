using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Expert
{
    public class FrozenMaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 28;

            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;

            Item.damage = 1225;
            Item.DamageType = DamageClass.Generic;
            Item.knockBack = 4f;

            Item.expert = true;
            Item.value = Item.sellPrice(gold: 15);
        }

        public override bool WeaponPrefix() => false;
        public override bool MagicPrefix() => false;
        public override bool RangedPrefix() => false;
        public override bool MeleePrefix() => false;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().FrozenMaw = true;
            player.GetModPlayer<TranscendencePlayer>().FrozenMawDamage = Item.damage;
        }
    }
}
