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
using TranscendenceMod.Items.Consumables.Boss;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Weapons.Melee;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class Atmospheron : TaskUIElement
    {
        public override Texture2D Icon => ModContent.Request<Texture2D>("TranscendenceMod/NPCs/Boss/Dragon/WindDragon_Head_Boss").Value;
        public override TaskIDs type => TaskIDs.Atmospheron;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().AtmospheronUnlock;

        public override float x => -75f;

        public override float y => 150f;

        public override string col => "4fdfff";

        public override CategoryIDs category => CategoryIDs.EaM;
    }
}

