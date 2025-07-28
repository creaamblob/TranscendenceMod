using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.NPCShops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;

namespace TranscendenceMod.Items.Tools
{
    public class WetPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 3f;

            Item.width = 42;
            Item.height = 42;

            Item.useTime = 3;
            Item.useAnimation = 12;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 17, silver: 50);
            Item.rare = ModContent.RarityType<Brown>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.pick = 220;
            Item.useTurn = true;
            Item.tileBoost += 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ReaverShark)
            .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 12)
            .AddIngredient(ItemID.GoldBar, 18)
            .AddTile(ModContent.TileType<Oceation>())
            .Register();
        }
    }
}
