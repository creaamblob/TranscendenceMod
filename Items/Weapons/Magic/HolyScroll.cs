using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class HolyScroll : ModItem
    {
        int proj = ModContent.ProjectileType<AngelicLaser_Friendly>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 138;
            Item.mana = 155;
            Item.knockBack = 1;
            Item.channel = true;

            Item.width = 12;
            Item.height = 26;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.sellPrice(gold: 17, silver: 50);
            Item.rare = ModContent.RarityType<ModdedPurple>();
            Item.shoot = proj;
            Item.shootSpeed = 1;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[proj] == 0;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 8; i++)
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.3f), proj, damage, knockback, player.whoAmI);
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<HolyScroll_HoldOut>()] == 0)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero,
                    ModContent.ProjectileType<HolyScroll_HoldOut>(), Item.damage, Item.knockBack, player.whoAmI);
            }
        }
    }
    public class HolyScroll_HoldOut : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = int.MaxValue;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, 1f, 0.7f, 0.2f);

            if (player.HeldItem.type != ModContent.ItemType<HolyScroll>() || player.GetModPlayer<TranscendencePlayer>().CannotUseItems || player.dead)
                Projectile.Kill();

            Projectile.direction = player.Center.X > Projectile.Center.X ? 1 : -1;
            Projectile.spriteDirection = -Projectile.direction;
            Projectile.Center = player.Center + new Vector2(25 * player.direction, -5);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            Main.instance.DrawCacheProjsOverPlayers.Add(index);
        }
    }
}