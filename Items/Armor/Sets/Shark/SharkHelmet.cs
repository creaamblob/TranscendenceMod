using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Other;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.BigTiles.Furniture;

namespace TranscendenceMod.Items.Armor.Sets.Shark
{
    [AutoloadEquip(EquipType.Head)]
    public class SharkHelmet : ModItem
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
            Item.defense = 8;
            Item.value = Item.sellPrice(gold: 7, silver: 50);
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SharkChestplate>() && legs.type == ModContent.ItemType<SharkBoots>();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.2f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().SharkscaleSetWear = true;
            player.fishingSkill += 25;

            player.GetAttackSpeed(DamageClass.Melee) += 0.3f;
            player.setBonus = Language.GetTextValue("Mods.TranscendenceMod.Messages.Tooltips.ArmorSetBonuses.Shark");
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.AnglerHat)
            .AddIngredient(ItemID.SharkFin, 12)
            .AddIngredient(ModContent.ItemType<PoseidonsTide>(), 6)
            .AddIngredient(ModContent.ItemType<SoulOfKnight>(), 2)
            .AddTile(ModContent.TileType<Oceation>())
            .Register();
        }
    }
}
