using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials.LargeRecipes;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items.Accessories.Offensive
{
    public class LaserSightLens : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.height = 18;
            Item.width = 18;

            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ModContent.RarityType<Brown>();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<TranscendencePlayer>().NucleusLensSocial = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TranscendencePlayer>().NucleusLens = true;
            if (!hideVisual)
                player.GetModPlayer<TranscendencePlayer>().NucleusLensSocial = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ModKeybind mkb = TranscendenceWorld.RetLensKeybind;
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
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ReconScope)
            .AddIngredient(ItemID.RangerEmblem)
            .AddIngredient(ItemID.UltrabrightHelmet)
            .AddIngredient(ModContent.ItemType<ElectricalComponent>(), 2)
            .AddIngredient(ItemID.SoulofSight, 15)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
}
