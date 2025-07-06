using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TranscendenceMod.Items.Consumables.Placeables;

namespace TranscendenceMod.Miscanellous.UI.Achievements.Tasks
{
    public class SeraphForge : TaskUIElement
    {
        public override Texture2D Icon => TextureAssets.Item[ModContent.ItemType<StarcraftedForgeItem>()].Value;
        public override TaskIDs type => TaskIDs.StarForge;
        public override bool Unlocked => Main.LocalPlayer.GetModPlayer<ModAchievementsHelper>().SeraphForgeUnlock;

        public override float x => 100f;

        public override float y => 75f;

        public override string col => "5a6c9a";

        public override CategoryIDs category => CategoryIDs.Prog;
    }
}

