using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Dusts
{
    public class Rainbow : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
        }
        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, TorchID.Rainbow);
            dust.color = Main.DiscoColor;
            return base.Update(dust);
        }
    }
}
