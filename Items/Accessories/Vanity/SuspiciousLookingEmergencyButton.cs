using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Vanity
{
    public class SuspiciousLookingEmergencyButton : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            SetupDrawing();
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.buyPrice(platinum: 25);
            Item.rare = ModContent.RarityType<MidnightBlue>();
            Item.master = true;
            Item.accessory = true;
            Item.vanity = true;
            Item.GetGlobalItem<TranscendenceItem>().SeraphDifficultyItem = true;
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().Sussy = true;
            player.AddBuff(ModContent.BuffType<AmogusTrans>(), 1);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<AmogusTrans>(), 1);
            player.GetModPlayer<TranscendencePlayer>().Sussy = true;
        }
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Head", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_Legs", EquipType.Legs, this);
            }
        }
        private void SetupDrawing()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                int head = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
                int body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
                int legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

                ArmorIDs.Body.Sets.HidesTopSkin[body] = true;
                ArmorIDs.Body.Sets.HidesBottomSkin[body] = true;
                ArmorIDs.Legs.Sets.HidesBottomSkin[legs] = true;
                ArmorIDs.Legs.Sets.HidesTopSkin[legs] = true;
                ArmorIDs.Head.Sets.DrawHead[head] = false;
            }
        }
    }
}
