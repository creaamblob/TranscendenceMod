using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.BigTiles.Furniture;

namespace TranscendenceMod.Items.Armor.Sets.Shark
{
    [AutoloadEquip(EquipType.Legs)]
    public class SharkBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ModContent.RarityType<Brown>();
            Item.defense = 4;
            Item.value = Item.sellPrice(gold: 7, silver: 50);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 0.15f;
            player.moveSpeed += 0.2f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.AnglerPants)
            .AddIngredient(ItemID.SharkFin, 8)
            .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 4)
            .AddIngredient(ModContent.ItemType<SoulOfKnight>(), 2)
            .AddTile(ModContent.TileType<Oceation>())
            .Register();
        }
    }
}
