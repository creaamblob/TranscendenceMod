using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.Assets.Gore.CelestialSeraph
{
    public abstract class BaseSeraphGore : ModGore
    {
        public int timer;
        public override bool Update(Terraria.Gore gore)
        {
            gore.position += gore.velocity;
            timer++;
            if (timer > 300)
                gore.alpha += 5;
            else
            {
                if (gore.alpha > 0)
                    gore.alpha -= 5;
            }
            if (gore.alpha > 250 && timer > 300)
                gore.active = false;
            return false;
        }
    }
}
