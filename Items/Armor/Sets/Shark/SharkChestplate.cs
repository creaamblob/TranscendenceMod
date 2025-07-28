using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Movement;
using TranscendenceMod.Items.Accessories.Other;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.BigTiles.Furniture;

namespace TranscendenceMod.Items.Armor.Sets.Shark
{
    [AutoloadEquip(EquipType.Body)]
    public class SharkChestplate : ModItem
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
            Item.defense = 12;
            Item.value = Item.sellPrice(gold: 7, silver: 50);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.2f;
            player.GetArmorPenetration(DamageClass.Melee) += 15;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.AnglerVest)
            .AddIngredient(ItemID.SharkFin, 16)
            .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 8)
            .AddIngredient(ModContent.ItemType<SoulOfKnight>(), 3)
            .AddIngredient(ItemID.SharkToothNecklace)
            .AddTile(ModContent.TileType<Oceation>())
            .Register();
        }
    }
}
