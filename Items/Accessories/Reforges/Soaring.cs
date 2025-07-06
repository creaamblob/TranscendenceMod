using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Reforges
{
    public class Soaring : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;
        public override float RollChance(Item item)
        {
            return 0.275f;
        }
        public override void ApplyAccessoryEffects(Player player)
        {
            player.wingTimeMax = (int)(player.wingTimeMax * 1.06f);
            player.rocketTime = (int)(player.rocketTimeMax * 1.06f);
        }
        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1f * 0.11f;
        }
    }
}