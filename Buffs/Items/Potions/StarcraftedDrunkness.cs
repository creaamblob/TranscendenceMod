﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Buffs.Items.Potions
{
    public class StarcraftedDrunkness : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<TranscendencePlayer>().StarcraftedDrunk = true;
        }
    }
}
