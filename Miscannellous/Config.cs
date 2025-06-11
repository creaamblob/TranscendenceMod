using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TranscendenceMod
{

    [BackgroundColor(35, 3, 18, 200)]
    public class TranscendenceConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Header("Configuration")]
        [BackgroundColor(55, 55, 55, 50)]

        [DefaultValue(true)]
        public bool PhotosensitivityMode { get; set; }
    }
}
