using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Volcanic : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<VolcanicRemains>()].Value;
        public override TaskIDs type => TaskIDs.Volcanic;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().VolcanicUnlock;

        public override float x => -100f;

        public override float y => -25f;

        public override string col => "ff6700";

        public override CategoryIDs category => CategoryIDs.EaM;
    }
}

