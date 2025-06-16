using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Accessories.Movement.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class MeteorJetpack : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.width = 24;
            Item.height = 16;
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 7, silver: 50);
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(45, 5, 0.75f);
        }
        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = player.wingTime > 0 ? 6 : 0;
            acceleration = 0.575f;
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0f;
            ascentWhenRising = 0.225f;
            maxCanAscendMultiplier = 0.7f;
            maxAscentMultiplier = 1f;
            constantAscend = 0.2125f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.MeteoriteBar, 20)
            .AddIngredient(ItemID.SunplateBlock, 20)
            .AddIngredient(ItemID.FallenStar, 25)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
