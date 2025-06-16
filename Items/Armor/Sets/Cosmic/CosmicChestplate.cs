using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles.Furniture;

namespace TranscendenceMod.Items.Armor.Sets.Cosmic
{
    [AutoloadEquip(EquipType.Body)]
    public class CosmicChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            var EquipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.HidesTopSkin[EquipSlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[EquipSlot] = true;
            ArmorIDs.Body.Sets.HidesArms[EquipSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ModContent.RarityType<MidnightBlue>();
            Item.defense = 28;
            Item.value = Item.sellPrice(gold: 25);
        }
        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.18f;
            player.maxMinions += 1;
            player.whipRangeMultiplier += 0.2f;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "";
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StarcraftedAlloy>(), 4)
            .AddIngredient(ModContent.ItemType<AetherRootItem>(), 18)
            .AddTile(ModContent.TileType<StarcraftedForge>())
            .Register();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
