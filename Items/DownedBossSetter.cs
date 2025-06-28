using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;

namespace TranscendenceMod.Items
{
    public class DownedBossSetter : ModItem
    {
        public int CurSelection = 0;
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Mech;

            Item.width = 18;
            Item.height = 18;

            Item.rare = ModContent.RarityType<Brown>();

        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);

            var tt = new TooltipLine(Mod, "DownedSelection", "Selected: " + text);
            tt.OverrideColor = Color.Gold;

            tooltips.Add(tt);
        }

        public override bool AltFunctionUse(Player player) => true;

        private string text = "";
        public override bool? UseItem(Player player)
        {
            if (player.ItemAnimationJustStarted)
            {

                if (player.altFunctionUse == 2)
                {
                    CurSelection++;

                    switch (CurSelection)
                    {
                        case 0: text = "Frost Serpent"; break;
                        case 1: text = "Atmospheron"; break;
                        case 2: text = "Project Nucleus"; break;
                        case 3: text = "Celestial Seraph"; break;
                        case 4: CurSelection = 0; goto case 0;
                    }

                    Main.NewText("Selected: " + text);
                }
                else
                {
                    bool downed = false;
                    switch (CurSelection)
                    {
                        case 0: downed = TranscendenceWorld.DownedFrostSerpent = !TranscendenceWorld.DownedFrostSerpent; break;
                        case 1: downed = TranscendenceWorld.DownedWindDragon = !TranscendenceWorld.DownedWindDragon; break;
                        case 2: downed = TranscendenceWorld.DownedNucleus = !TranscendenceWorld.DownedNucleus; break;
                        case 3: downed = TranscendenceWorld.DownedSpaceBoss = !TranscendenceWorld.DownedSpaceBoss; break;
                    }
                    Main.NewText("Flags for " + text + " set to " + downed.ToString() + "!");
                }

            }
            return base.UseItem(player);
        }
    }
}