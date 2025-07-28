using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.Items.Weapons.Ranged
{
    public class SpaceBow : ModItem
    {
        public int proj = ModContent.ProjectileType<CoolBow>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 275;
            Item.knockBack = 2;
            Item.DamageType = DamageClass.Ranged;

            Item.width = 34;
            Item.height = 46;
            Item.useAmmo = AmmoID.Arrow;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.autoReuse = true;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.reuseDelay = 30;

            Item.value = Item.sellPrice(gold: 35);
            Item.rare = ModContent.RarityType<CosmicRarity>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.crit = 15;

            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20f;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override bool CanConsumeAmmo(Item ammo, Player player) => false;
        public override void ModifyShootStats(Player player, ref Vector2 position,
            ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => type = ProjectileID.None;
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[proj] > 0)
                return false;
            else return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[proj] < 1)
            {
                Projectile.NewProjectile(source, position, velocity, proj, damage, knockback, -1, 0, 0, 12);
            }

            return false;
        }
    }

    public class CoolBow : ModProjectile
    {
        public int ChargeTimeForShot = 25;
        public int MaxCharge = 120;
        public int RedFlashTimer;
        public int TimeSinceSpawn;
        Player player;
        public bool FirstShot;
        public int Timer2;
        public int TurnTimer;
        public static Item VanillaIsSoPicky = null;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.aiStyle = -1;

            Projectile.light = 0.5f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 90;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Items/Weapons/Ranged/SpaceBow").Value;
            if (TimeSinceSpawn > 5)
            {
                if (Main.MouseWorld.Distance(player.Center) > 20)
                {
                    Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + player.fullRotation, sprite.Size() * 0.5f, 1,
                        Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);
                    if (Projectile.ai[1] >= MaxCharge)
                    {
                        RedFlashTimer++;
                        Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Red, Color.Transparent, (float)Math.Sin(RedFlashTimer / 4)), Projectile.rotation, sprite.Size() * 0.5f, 1f,
                            Main.MouseWorld.X < Projectile.Center.X ? SpriteEffects.FlipVertically : SpriteEffects.None);
                    }
                }
            }
            return false;
        }
        public static void VanillaAss()
        {
            VanillaIsSoPicky = new Item();
            VanillaIsSoPicky.SetDefaults(ItemID.AshWoodBow, true);
        }
        public override void OnSpawn(IEntitySource source)
        {
            FirstShot = true;
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (player.HeldItem.type != ModContent.ItemType<SpaceBow>())
                Projectile.Kill();

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) + Projectile.velocity * 3.5f;
            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            VanillaAss();

            Projectile.spriteDirection = (int)Projectile.rotation;
            Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 3.25f;
            Projectile.rotation = Projectile.AngleTo(Main.MouseWorld);
            player.GetModPlayer<TranscendencePlayer>().CannotUseItemsTimer = 10;

            if (Projectile.ai[1] == ChargeTimeForShot)
                SoundEngine.PlaySound(SoundID.MaxMana);

            if (Main.MouseWorld.Distance(player.Center) > 10)
            {
                player.direction = (Main.MouseWorld.X > player.Center.X ? 1 : -1);
                player.itemTime = 2;
                player.itemAnimation = 2;

                if (player.controlUseItem)
                {
                    if (Projectile.ai[1] < ChargeTimeForShot && TimeSinceSpawn > 5)
                        TranscendenceUtils.DustRing(Projectile.Center + Projectile.velocity * 10, 5, ModContent.DustType<NovaDust>(), 1, Color.White, 1);

                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.ThreeQuarters, (player.MountedCenter - Projectile.Center).ToRotation() + (player.direction == 1 ? MathHelper.PiOver4 * 3 : MathHelper.PiOver4));
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

                    Projectile.timeLeft = 25;
                    if (Projectile.ai[1] < MaxCharge) Projectile.ai[1]++;
                }
                else if (Projectile.ai[1] > ChargeTimeForShot && player.releaseUseItem && (FirstShot || player.HasAmmo(VanillaIsSoPicky)))
                {
                    SoundEngine.PlaySound(SoundID.Item102, Projectile.Center);
                    player.PickAmmo(VanillaIsSoPicky, out int projToShoot, out float speed, out int damage, out float knockBack, out int usedAmmoItemId, false);

                    int dmg = (int)MathHelper.Lerp(Projectile.damage * 0.25f, Projectile.damage * 2.25f, Projectile.ai[1] / MaxCharge);

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld) * speed,
                        ModContent.ProjectileType<OrionsHelixFriendly>(), dmg, Projectile.knockBack, -1, 0, 0, 16);

                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld) * speed,
                        ModContent.ProjectileType<OrionsHelixFriendly>(), dmg, Projectile.knockBack, -1, 0, 0, -16);

                    if (Projectile.ai[1] >= MaxCharge)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld).RotatedBy(MathHelper.PiOver4 / 2) * speed,
                            ModContent.ProjectileType<OrionsHelixFriendly>(), dmg, Projectile.knockBack, -1, 0, 0, 16);

                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld).RotatedBy(MathHelper.PiOver4 / 2) * speed,
                            ModContent.ProjectileType<OrionsHelixFriendly>(), dmg, Projectile.knockBack, -1, 0, 0, -16);

                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld).RotatedBy(-MathHelper.PiOver4 / 2) * speed,
                            ModContent.ProjectileType<OrionsHelixFriendly>(), dmg, Projectile.knockBack, -1, 0, 0, 16);

                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld).RotatedBy(-MathHelper.PiOver4 / 2) * speed,
                            ModContent.ProjectileType<OrionsHelixFriendly>(), dmg, Projectile.knockBack, -1, 0, 0, -16);
                    }

                    FirstShot = false;

                    Projectile.timeLeft = 60;
                    Projectile.ai[1] = 0;
                }
            }
        }
    }
}