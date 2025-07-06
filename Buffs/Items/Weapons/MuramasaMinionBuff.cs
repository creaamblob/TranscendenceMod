using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Weapons.Summoner;

namespace TranscendenceMod.Buffs.Items.Weapons
{
    public class MuramasaMinionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            Main.debuff[Type] = false;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MuramasaSummon>()] > 0)
                player.buffTime[buffIndex] = 30;
        }
    }
}
