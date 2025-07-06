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
using TranscendenceMod.Items.Tools.Generic.Hardmetal;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Headless : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ItemID.ZombieArm].Value;
        public override TaskIDs type => TaskIDs.Headless;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().HeadlessUnlock;

        public override float x => -75f;

        public override float y => -50f;

        public override string col => "df1032";

        public override CategoryIDs category => CategoryIDs.EaM;
    }
}

