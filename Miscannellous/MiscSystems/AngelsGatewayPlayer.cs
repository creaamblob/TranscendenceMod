using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Dusts;

namespace TranscendenceMod
{
    public class AngelsGatewayPlayer : ModPlayer
    {
        // Will add a better approach once I add more content to this place
        public int ZoneAngelGateway;
        public Vector2 AnchorPosition;
        public int AnchorUpdatePosition;
        public int SeraphRitualActive;

        public override void ResetEffects()
        {
            base.ResetEffects();

            if (ZoneAngelGateway > 0)
                ZoneAngelGateway--;

            if (SeraphRitualActive > 0)
                SeraphRitualActive--;

            if (SeraphTileDrawingSystem.PhaseThroughTimer > 0)
                SeraphTileDrawingSystem.PhaseThroughTimer--;

            if (++AnchorUpdatePosition % 30 == 0)
                AnchorPosition = Player.Center;
        }

        public override void PostUpdateMiscEffects()
        {
            base.PostUpdateMiscEffects();

            if (ZoneAngelGateway > 0)
            {
                if (Main.rand.NextBool(20))
                    Dust.NewDust(AnchorPosition + Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(200f, 800f), 1, 1, ModContent.DustType<PlayerCosmicBlood>(),
                        0, 0, 0, Color.Lerp(Color.DeepSkyBlue, Color.Magenta, Main.rand.NextFloat(0f, 1.5f)), 1f);

                SkyManager.Instance.Activate("TranscendenceMod:AngelsGatewaySky", Player.Center);
                SeraphTileDrawingSystem.PhaseThroughTimer = 5;
                Player.gravity *= 0.33f;
            }
            else SkyManager.Instance.Deactivate("TranscendenceMod:AngelsGatewaySky");

        }

        public override void PreUpdateMovement()
        {
            base.PreUpdateMovement();
        }
    }
}

