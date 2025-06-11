using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.TerrestrialSecond;

namespace TranscendenceMod.Items.Materials
{
    public class ExtraTerrestrialString : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }
        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 20;
            Item.value = Item.buyPrice(silver: 5);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.maxStack = 9999;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Lighting.AddLight(Item.Center, 0.3f, 0f, 0.3f);
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override void AddRecipes()
        {
            CreateRecipe(3)
            .AddIngredient(ItemID.Cobweb, 3)
            .AddIngredient(ModContent.ItemType<PulverizedPlanet>(), 2)
            .AddTile(ModContent.TileType<ExtraTerrestrialLoom>())
            .Register();
        }
    }
}
