using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Crean;

namespace TranscendenceMod.Items.Weapons
{
    public class CreanStaff : ModItem
    {
        int proj = ModContent.ProjectileType<CreanStaffProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Generic;
            Item.damage = 300;
            Item.knockBack = 2f;
            Item.crit = 10;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 36f;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.None;
            Item.UseSound = SoundID.Item41;

            Item.width = 40;
            Item.height = 33;
            Item.noMelee = true;
            Item.channel = true;
            Item.noUseGraphic = true;

            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<CosmicRarity>();

        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[proj] == 0 && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero,
                    proj, Item.damage, Item.knockBack, player.whoAmI);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.RainbowRod)
            .AddIngredient(ModContent.ItemType<SpritersBrush>())
            .AddIngredient(ItemID.SoulofFlight, 125)
            .AddIngredient(ItemID.SoulofLight, 50)
            .AddIngredient(ItemID.LunarOre, 5)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
        }
    }
    public class CreanStaffProj : ModProjectile
    {
        public int Combo;
        public int TimeSinceSpawn;
        public int dir;
        Player player;
        public override string Texture => "TranscendenceMod/Items/Weapons/CreanStaff";
        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 15;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool? CanDamage() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            if (TimeSinceSpawn > 5)
            {
                if (Main.MouseWorld.Distance(player.Center) > 75)
                {
                    Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + player.fullRotation + MathHelper.PiOver2, sprite.Size() * 0.5f, 1f,
                        SpriteEffects.None);
                }
            }
            return false;
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (player.HeldItem.type == ModContent.ItemType<CreanStaff>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems && !player.dead)
                Projectile.timeLeft = 5;
            else return;

            Vector2 pos = player.Center + Projectile.velocity * 5.65f;
            Projectile.Center = pos;
            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            if (TimeSinceSpawn < 5)
                Projectile.ai[1] = -1;

            float rotation = (player.Center - Main.MouseWorld).ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4 - player.fullRotation;

            Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * 6.65f;
            Projectile.rotation = rotation;

            Vector2 vec = Projectile.Center + Projectile.velocity * 4f;
            Vector2 vel = Projectile.DirectionTo(Main.MouseWorld);



            int snatcher = ModContent.ProjectileType<CreanSnatcher>();
            int cog = ModContent.ProjectileType<CreanCog>();
            int stargazer = ModContent.ProjectileType<CreanStargazer>();

            if (player.ownedProjectileCounts[snatcher] == 0 && TranscendenceWorld.DownedFrostSerpent)
            {
                for (int i = 0; i < 5; i++)
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, snatcher, player.HeldItem.damage / 4, 2f, player.whoAmI, 0f, i, 5f);
            }
            if (player.ownedProjectileCounts[cog] == 0 && TranscendenceWorld.DownedNucleus)
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, vel * 8f, cog, player.HeldItem.damage, 3f, player.whoAmI);
            }
            if (player.ownedProjectileCounts[stargazer] < 2 && TranscendenceWorld.DownedSpaceBoss)
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, stargazer, player.HeldItem.damage, 2f, player.whoAmI, 0f, player.ownedProjectileCounts[stargazer], 2f);
            }

            if (Main.MouseWorld.Distance(player.Center) > 75)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation - MathHelper.PiOver4);
                dir = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                player.direction = dir;


                if (Projectile.ai[2] > 0)
                    Projectile.ai[2]--;

                if (player.controlUseItem && Projectile.ai[2] == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item67, Projectile.Center);

                    float speed = 5f;
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec, vel * speed, ModContent.ProjectileType<CreanLaser>(), player.HeldItem.damage / 2, Projectile.knockBack, player.whoAmI, 0f, 0f, 1f);
                    int p2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), vec, vel * speed, ModContent.ProjectileType<CreanLaser>(), player.HeldItem.damage / 2, Projectile.knockBack, player.whoAmI, 0f, 1f, -1f);
                    Main.projectile[p].extraUpdates += TranscendenceWorld.DownedWindDragon ? 8 : 4;
                    Main.projectile[p2].extraUpdates += TranscendenceWorld.DownedWindDragon ? 8 : 4;


                    if (player.HeldItem.GetGlobalItem<ModifiersItem>().ChargerCharge > 0)
                    {
                        player.HeldItem.GetGlobalItem<ModifiersItem>().ChargerCharge -= 0.05f;
                        player.HeldItem.GetGlobalItem<ModifiersItem>().ChargeCD = 60;
                    }

                    if (player.GetModPlayer<TranscendencePlayer>().CreanStaffClick == 0)
                        player.GetModPlayer<TranscendencePlayer>().CreanStaffClick = 40;

                    Projectile.ai[1]++;
                    Projectile.ai[2] = player.HeldItem.useAnimation;
                }
            }
        }
    }
}