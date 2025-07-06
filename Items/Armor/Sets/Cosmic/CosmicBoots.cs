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
    [AutoloadEquip(EquipType.Legs)]
    public class CosmicBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            var EquipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.OverridesLegs[EquipSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ModContent.RarityType<MidnightBlue>();
            Item.defense = 15;
            Item.value = Item.sellPrice(gold: 20);
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.25f;
            player.maxRunSpeed *= 1.25f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<StarcraftedAlloy>(), 3)
            .AddIngredient(ModContent.ItemType<AetherRootItem>(), 12)
            .AddTile(ModContent.TileType<StarcraftedForge>())
            .Register();
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
