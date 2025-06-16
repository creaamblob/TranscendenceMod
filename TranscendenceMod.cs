using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Cosmetics;
using TranscendenceMod.Miscannellous.Skies;

namespace TranscendenceMod
{
    public class TranscendenceMod : Mod
    {
        public const string ASSET_PATH = "TranscendenceMod/Miscannellous/Assets";
        public static TranscendenceMod Instance { get; set; }
        public TranscendenceMod() => Instance = this;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                SkyManager.Instance["TranscendenceMod:CelestialSeraph"] = new SpaceBossSky();
                SkyManager.Instance["TranscendenceMod:SpaceMonolith"] = new SpaceMonolithSky();
                SkyManager.Instance["TranscendenceMod:DragonSky"] = new DragonSky();
                SkyManager.Instance["TranscendenceMod:FrostSky"] = new FrostSky();
                SkyManager.Instance["TranscendenceMod:AngelsGatewaySky"] = new AngelGatewaySky();
            }
            if (Main.netMode != NetmodeID.Server)
            {
                Asset<Effect> dyeShader = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/CreamsShader");
                GameShaders.Armor.BindShader(ModContent.ItemType<ShimmerLightDye>(), new ArmorShaderData(dyeShader, "Test2")).UseColor(0.09f, 0.21f, 0.54f);

                Asset<Effect> flashbangShader = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/Flashbang");
                Filters.Scene["TranscendenceMod:FlashbangShader"] = new Filter(new ScreenShaderData(flashbangShader, "FlashbangTechnique2").UseColor(Color.White), EffectPriority.VeryHigh);

                Asset<Effect> staticShader = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/ScreenStatic");
                Filters.Scene["TranscendenceMod:Static"] = new Filter(new ScreenShaderData(staticShader, "StaticTechnique2"), EffectPriority.VeryHigh);

                Asset<Effect> blind = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/BlindShader");
                Filters.Scene["TranscendenceMod:Blindness"] = new Filter(new ScreenShaderData(blind, "BlindTechnique2"), EffectPriority.VeryHigh);

                Asset<Effect> retlens = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/NucleusLensShader");
                Filters.Scene["TranscendenceMod:RetLens"] = new Filter(new ScreenShaderData(retlens, "NLensTechnique2"), EffectPriority.VeryHigh);

                Asset<Effect> saturationShader = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/SaturationScreen");
                Filters.Scene["TranscendenceMod:SaturationShader"] = new Filter(new ScreenShaderData(saturationShader, "SaturationTechnique2"), EffectPriority.VeryHigh);

                Asset<Effect> desaturationShader = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/DesaturationShader");
                Filters.Scene["TranscendenceMod:DesaturationShader"] = new Filter(new ScreenShaderData(desaturationShader, "DeSaturationTechnique2"), EffectPriority.VeryHigh);

                Asset<Effect> inverseShader = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/InverseScreen");
                Filters.Scene["TranscendenceMod:InverseShader"] = new Filter(new ScreenShaderData(inverseShader, "InverseTechnique2"), EffectPriority.VeryHigh);

                Asset<Effect> hotShader = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/VeryHotScreen");
                Filters.Scene["TranscendenceMod:HotScreen"] = new Filter(new ScreenShaderData(hotShader, "WarmTechnique2"), EffectPriority.VeryHigh);

                Asset<Effect> coldScreen = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/VeryColdScreen");
                Filters.Scene["TranscendenceMod:ColdScreen"] = new Filter(new ScreenShaderData(coldScreen, "ColdTechnique2"), EffectPriority.VeryHigh);

                Asset<Effect> vignetteScreen = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/ScreenVignetteShader");
                Filters.Scene["TranscendenceMod:ScreenVignette"] = new Filter(new ScreenShaderData(vignetteScreen, "ScreenVignetteTechnique2"), EffectPriority.VeryHigh);

                Asset<Effect> flip = Assets.Request<Effect>("Miscannellous/Assets/Shaders/Effects/ScreenFlip");
                Filters.Scene["TranscendenceMod:ScreenFlip"] = new Filter(new ScreenShaderData(flip, "FlipTechnique2"), EffectPriority.VeryHigh);
            }
        }
    }
}