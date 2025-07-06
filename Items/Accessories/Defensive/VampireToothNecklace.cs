using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Movement;
using TranscendenceMod.Items.Accessories.Offensive.EoL;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Accessories.Defensive
{
    [AutoloadEquip(EquipType.Neck)]
    public class VampireToothNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Red;
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 75);
        }

        public override bool MeleePrefix() => false;
        public override bool WeaponPrefix() => false;

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ModKeybind mkb = TranscendenceWorld.InfectionAccessoryKeyBind;
            if (!Main.dedServ && mkb != null)
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
            if (!Main.dedServ)
            {
                Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer mpr);

                if (mpr == null)
                    return;

                int amount = mpr.VampireHealAmount;
                TooltipLine line = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Text.Contains("(Healed Amount)"));

                if (line != null)
                    line.Text = line.Text.Replace("(Healed Amount)", amount.ToString());
            }
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().Vampire = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<eoltransform>())
                return incomingItem.type != ModContent.ItemType<eoltransform>();

            if (equippedItem.type == ModContent.ItemType<CorruptedWanderingKit>())
                return incomingItem.type != ModContent.ItemType<CorruptedWanderingKit>();

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.StingerNecklace)
            .AddIngredient(ItemID.SoulofNight, 30)
            .AddIngredient(ItemID.CrimtaneBar, 75)
            .AddIngredient(ItemID.FragmentNebula, 30)
            .AddIngredient(ItemID.LunarBar, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
