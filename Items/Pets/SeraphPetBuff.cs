using Terraria;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Pets
{
    public class SeraphPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            bool reference = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref reference, ModContent.ProjectileType<SeraphPet>());
        }
    }
}
