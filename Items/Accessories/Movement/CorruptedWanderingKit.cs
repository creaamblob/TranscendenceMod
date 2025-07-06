using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Accessories.Defensive;
using TranscendenceMod.Items.Accessories.Offensive.EoL;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Wings)]
    public class CorruptedWanderingKit : ModItem
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ModKeybind mkb = TranscendenceWorld.InfectionAccessoryKeyBind;
            if (!Main.dedServ && mkb != null)
            {
                for (int i = 0; i < 3; i++)
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
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<VampireToothNecklace>())
                return incomingItem.type != ModContent.ItemType<VampireToothNecklace>();

            if (equippedItem.type == ModContent.ItemType<CorruptedWanderingKit>())
                return incomingItem.type != ModContent.ItemType<eoltransform>();

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.width = 25;
            Item.height = 30;
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 75);
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(600, 12, 0.5f, true, 10);
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 12f;
            acceleration = 0.33f;

            if (player.controlDown && player.controlJump && player.wingTime > 0)
            {
                acceleration = 1.5f;
                speed = 10;

                player.position.Y -= player.velocity.Y;

                if (player.velocity.Y > 0.1f)
                    player.velocity.Y = 0.1f;

                else if (player.velocity.Y < -0.1f)
                    player.velocity.Y = -0.1f;
            }
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.8f;
            ascentWhenRising = 0.325f;
            maxCanAscendMultiplier = 1.5f;
            maxAscentMultiplier = 4f;
            constantAscend = 0.175f;

            if (player.controlUp && player.controlJump)
            {
                constantAscend = 0.25f;
                ascentWhenRising = 0.5f;
                maxCanAscendMultiplier = 2.25f;
                maxAscentMultiplier = 6f;
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().CorruptWanderingKit = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.TerrasparkBoots)
            .AddIngredient(ItemID.SoulofNight, 30)
            .AddIngredient(ItemID.DemoniteBar, 75)
            .AddIngredient(ItemID.FragmentVortex, 30)
            .AddIngredient(ItemID.LunarBar, 10)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
