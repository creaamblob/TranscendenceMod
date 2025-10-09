using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class VoidBiome : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<VoidSeeds>()].Value;
        public override TaskIDs type => TaskIDs.VoidBiome;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().VoidBiomeUnlock;

        public override float x => 0f;

        public override float y => -1125f;

        public override string col => "0e716b";

        public override CategoryIDs category => CategoryIDs.EaM;
    }
}

