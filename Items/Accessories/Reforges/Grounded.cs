using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Accessories.Reforges
{
    public class Grounded : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;
        public override float RollChance(Item item)
        {
            return 0.33f;
        }
        public override void ApplyAccessoryEffects(Player player)
        {
            player.runAcceleration *= 1.08f;
            player.wingTimeMax = (int)(player.wingTimeMax * 0.95f);
            player.rocketTime = (int)(player.rocketTimeMax * 0.95f);
        }
        public override void ModifyValue(ref float valueMult)
        {
            valueMult += 1f * 0.1f;
        }
    }
}