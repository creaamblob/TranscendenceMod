using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Materials.MobDrops
{
    public class SoulOfKnight : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 14;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ModContent.RarityType<Brown>();
            Item.maxStack = 9999;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
