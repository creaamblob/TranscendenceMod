using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Materials.MobDrops
{
    public class VolcanicRemains : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 12;
            Item.value = Item.sellPrice(silver: 7, copper: 50);
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);

            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
    }
}
