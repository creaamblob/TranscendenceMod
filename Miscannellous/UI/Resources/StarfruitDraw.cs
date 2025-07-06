using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TranscendenceMod.Miscannellous.UI.Resources
{
    public class StarfruitDraw : ModResourceOverlay
    {
        private Dictionary<string, Asset<Texture2D>> vanillaAssetCache = new();
        private Asset<Texture2D> heart, fancyBG, bar, barBG;
        string fancy = "Images/UI/PlayerResourceSets/FancyClassic/";
        string bars = "Images/UI/PlayerResourceSets/HorizontalBars/";
        public override void PostDrawResource(ResourceOverlayDrawContext context)
        {
            Asset<Texture2D> asset = context.texture;
            bool drawingBarsBG = CompareAssets(asset, bars + "HP_Panel_Middle");
            int starFruits = Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().EatenStarfruits;

            if (context.resourceNumber >= 1 * starFruits)
                return;

            if (asset == TextureAssets.Heart || asset == TextureAssets.Heart2)
                DrawHeart(context);

            else if (CompareAssets(asset, fancy + "Heart_Fill") || CompareAssets(asset, fancy + "Heart_Fill_B"))
                DrawHeart(context);

            //Why are life fruits called honey????
            else if (CompareAssets(asset, bars + "HP_Fill") || CompareAssets(asset, bars + "HP_Fill_Honey"))
                DrawBars(context);

            else if (CompareAssets(asset, fancy + "Heart_Middle") || CompareAssets(asset, fancy + "Heart_Right")
                || CompareAssets(asset, fancy + "Heart_Right_Fancy") || CompareAssets(asset, fancy + "Heart_Single_Fancy"))
            {
                DrawFancyBG(context);
            }

            else if (drawingBarsBG)
                DrawBarsBG(context);

            if (CompareAssets(asset, fancy + "Heart_Left"))
            {
                context.texture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Resources/StarfruitEmpty");
                context.source = context.texture.Frame();
                context.position += new Vector2(4, 4);
                context.Draw();
            }
        }

        private void DrawHeart(ResourceOverlayDrawContext context)
        {
            context.texture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Resources/StarfruitClassic");
            context.Draw();
        }

        private void DrawBars(ResourceOverlayDrawContext context)
        {
            context.texture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Resources/StarfruitBar");
            context.Draw();
        }

        private void DrawBarsBG(ResourceOverlayDrawContext context)
        {
            context.texture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Resources/StarfruitBarBG");
            context.position += new Vector2(0, 6);
            context.scale.Y = 0.5f;
            context.Draw();
        }

        private void DrawFancyBG(ResourceOverlayDrawContext context)
        {
            context.texture = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/Resources/StarfruitEmpty");
            Vector2 positionOffset;
            string fancyFolder = "Images/UI/PlayerResourceSets/FancyClassic/";

            if (context.resourceNumber == context.snapshot.AmountOfLifeHearts - 1)
            {
                // Final panel to draw has a special "Fancy" variant.  Determine whether it has panels to the left of it
                if (CompareAssets(context.texture, fancyFolder + "Heart_Single_Fancy"))
                    positionOffset = new Vector2(8, 8);

                else
                    positionOffset = new Vector2(8, 8);
            }

            else if (CompareAssets(context.texture, fancyFolder + "Heart_Middle"))
                positionOffset = new Vector2(0, 4);

            //Final one in the first row
            else
                positionOffset = new Vector2(0, 4);

            context.source = context.texture.Frame();
            context.position += positionOffset;
            context.Draw();
        }

        private bool CompareAssets(Asset<Texture2D> vanillaAsset, string v)
        {
            if (!vanillaAssetCache.TryGetValue(v, out var asset))
                asset = vanillaAssetCache[v] = Main.Assets.Request<Texture2D>(v);
            return vanillaAsset == asset;
        }
    }
}
