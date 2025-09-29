using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Tools.Compasses
{
    public abstract class Compass : ModItem
    {
        public Vector2 Pos;
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.width = 18;
            Item.height = 18;

            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item92;
            Item.holdStyle = ItemHoldStyleID.HoldLamp;

            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.autoReuse = true;

            Item.shoot = ProjectileID.PhantasmalSphere;
            Item.shootSpeed = 10;
        }
    }
}
