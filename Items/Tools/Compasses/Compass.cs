using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TranscendenceMod.Items.Tools.Compasses
{
    public class Compass : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.width = 18;
            Item.height = 18;

            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item92;
            Item.holdStyle = ItemHoldStyleID.HoldLamp;

            Item.value = Item.buyPrice(gold: 7, silver: 50);
            Item.autoReuse = true;

            Item.shoot = ProjectileID.PhantasmalSphere;
            Item.shootSpeed = 10;
        }
        public override void HoldItem(Player player)
        {
            Vector2 pos = player.SpawnX > 0 && player.SpawnY > 0 ? new Vector2(player.SpawnX, player.SpawnY) : new Vector2(Main.spawnTileX, Main.spawnTileY);

            if (player.velocity.Length() < 0.01f)
                Dust.QuickDustLine(player.Center + new Vector2(22 * player.direction, 4), player.Center + new Vector2(22 * player.direction, 4) + Vector2.One.RotatedBy(player.DirectionTo(pos * 16).ToRotation() - MathHelper.PiOver4) * 5, 50, Color.White);

            base.HoldItem(player);
        }
    }
}
