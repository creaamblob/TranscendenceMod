using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ID;

namespace TranscendenceMod
{
    public class VanillaResprites : ModSystem
    {
        public override void Load()
        {
            base.Load();

            //Apply sprite changes

            /*Valor*/
            Asset<Texture2D> valorIt = ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/VanillaResprites/Valor");
            Asset<Texture2D> valorPr = ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/VanillaResprites/ValorProj");

            TextureAssets.Item[ItemID.Valor] = valorIt;
            TextureAssets.Projectile[ProjectileID.Valor] = valorPr;
            /*//Valor*/

            /*Bunny Ears*/
            Asset<Texture2D> bunnyF = ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/VanillaResprites/BunnyFront");
            Asset<Texture2D> bunnyB = ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/VanillaResprites/BunnyBack");

            TextureAssets.ArmorHead[224] = bunnyF;
            TextureAssets.ArmorHead[253] = bunnyB;
        }
        public override void Unload()
        {
            base.Unload();

            //Revert sprite changes

            /*Valor*/
            Asset<Texture2D> valorIt = ModContent.Request<Texture2D>($"Terraria/Images/Item_3317");
            Asset<Texture2D> valorPr = ModContent.Request<Texture2D>($"Terraria/Images/Item_564");

            TextureAssets.Item[ItemID.Valor] = valorIt;
            TextureAssets.Projectile[ProjectileID.Valor] = valorPr;
            /*//Valor*/

            /*Bunny Ears*/
            Asset<Texture2D> bunnyF = ModContent.Request<Texture2D>($"Terraria/Images/Armor_Head_224");
            Asset<Texture2D> bunnyB = ModContent.Request<Texture2D>($"Terraria/Images/Armor_Head_253");

            TextureAssets.ArmorHead[224] = bunnyF;
            TextureAssets.ArmorHead[253] = bunnyB;
            /*//Bunny Ears*/
        }
    }
}

