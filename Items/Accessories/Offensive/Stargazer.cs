using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Offensive
{
    public class Stargazer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.DamageType = DamageClass.Generic;

            Item.height = 26;
            Item.width = 24;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ModContent.RarityType<ModdedPurple>();
        }
        public override bool WeaponPrefix() => false;
        public override bool MagicPrefix() => false;
        public override bool RangedPrefix() => false;
        public override bool MeleePrefix() => false;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().Stargazer = true;
            player.GetModPlayer<TranscendencePlayer>().stargazerDamage = Item.damage;
        }
    }
}
