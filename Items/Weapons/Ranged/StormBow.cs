using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class StormBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 240;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 48;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<Brown>();
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.crit = 18;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 25f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = position + new Vector2((float)Math.Sin(TranscendenceWorld.UniversalRotation * 4) * 125, i * 125);
                Projectile.NewProjectile(source, pos, new Vector2(pos.DirectionTo(Main.MouseWorld).X * 25, velocity.Y), type, damage, knockback, player.whoAmI);
            }
            return false;

        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextBool(5);

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = new Vector2(Main.MouseWorld.X, player.Center.Y - Main.screenHeight);
            type = ModContent.ProjectileType<StormbowArrow>();
        }
    }
}