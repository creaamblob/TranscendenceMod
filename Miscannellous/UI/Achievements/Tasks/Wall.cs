using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Items.Tools.Generic.Hardmetal;
using TranscendenceMod.Items.Weapons.Magic;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Wall : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.NpcHeadBoss[22].Value;
        public override TaskIDs type => TaskIDs.Wall;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().WallUnlock;

        public override float x => -75f;

        public override float y => 50f;

        public override string col => "9e254c";

        public override CategoryIDs category => CategoryIDs.Prog;
    }
}

