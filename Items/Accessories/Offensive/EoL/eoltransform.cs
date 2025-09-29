using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.InfectionAccessories;
using TranscendenceMod.Items.Accessories.Defensive;
using TranscendenceMod.Items.Accessories.Movement;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod.Items.Accessories.Offensive.EoL
{
    public class eoltransform : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            SetupDrawing();
        }

        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 25;

            Item.value = Item.buyPrice(gold: 75);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup(nameof(ItemID.HallowedMask))
            .AddIngredient(ModContent.ItemType<SunburntAlloy>(), 18)
            .AddIngredient(ItemID.SoulofLight, 50)
            .AddIngredient(ItemID.FragmentSolar, 50)
            .AddIngredient(ItemID.CrystalShard, 75)
            .AddIngredient(ModContent.ItemType<HeartOfTheQueen>())
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<VampireToothNecklace>())
                return incomingItem.type != ModContent.ItemType<VampireToothNecklace>();

            if (equippedItem.type == ModContent.ItemType<CorruptedWanderingKit>())
                return incomingItem.type != ModContent.ItemType<CorruptedWanderingKit>();

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ModKeybind mkb = TranscendenceWorld.InfectionAccessoryKeyBind;
            if (!Main.dedServ && mkb != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    List<string> keys = mkb.GetAssignedKeys();

                    if (keys.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder(10);
                        sb.Append(keys[0]);

                        TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Text.Contains("(Unbound Key)"));
                        if (line != null)
                            line.Text = line.Text.Replace("(Unbound Key)", sb.ToString());
                    }
                }
            }
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().ShowEolTransform = true;
            player.AddBuff(ModContent.BuffType<EolTrans>(), 1);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().EverglowingCrownEquipped = true;
            player.AddBuff(ModContent.BuffType<EolTrans>(), 1);
            player.GetModPlayer<TranscendencePlayer>().ShowEolTransform = !hideVisual;
        }
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Head", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Legs", EquipType.Legs, this);
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Back", EquipType.Back, this);

                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Day_Head", EquipType.Head, name: "Day");
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Day_Legs", EquipType.Legs, name: "Day");
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Day_Back", EquipType.Back, name: "Day");
            }
        }
        private void SetupDrawing()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                int head = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
                int body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
                int legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
                int back = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Back);

                int headDay = EquipLoader.GetEquipSlot(Mod, "Day", EquipType.Head);
                int legsDay = EquipLoader.GetEquipSlot(Mod, "Day", EquipType.Legs);
                int backDay = EquipLoader.GetEquipSlot(Mod, "Day", EquipType.Back);

                ArmorIDs.Body.Sets.HidesTopSkin[body] = true;
                ArmorIDs.Body.Sets.HidesBottomSkin[body] = true;
                ArmorIDs.Legs.Sets.HidesBottomSkin[legs] = true;
                ArmorIDs.Legs.Sets.HidesBottomSkin[legsDay] = true;
                ArmorIDs.Legs.Sets.HidesTopSkin[legs] = true;
                ArmorIDs.Legs.Sets.HidesTopSkin[legsDay] = true;
                ArmorIDs.Head.Sets.DrawHead[head] = false;
                ArmorIDs.Head.Sets.DrawHead[headDay] = false;
                ArmorIDs.Back.Sets.DrawInBackpackLayer[back] = true;
                ArmorIDs.Back.Sets.DrawInBackpackLayer[backDay] = true;
            }
        }
    }
}
