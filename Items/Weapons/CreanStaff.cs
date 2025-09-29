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
using static TranscendenceMod.TranscendenceWorld;

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

            Item.useTime = 35;
            Item.useAnimation = 35;
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
            .AddIngredient(ItemID.SapphireStaff)
            .AddIngredient(ItemID.RainbowRod)
            .AddIngredient(ModContent.ItemType<SpritersBrush>())
            .AddIngredient(ItemID.SoulofFlight, 125)
            .AddIngredient(ItemID.SoulofLight, 50)
            .AddIngredient(ItemID.Sapphire, 25)
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
        public float Recoil;
        Player player;
        public override string Texture => "TranscendenceMod/Items/Weapons/CreanStaff";
        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
        }
        public override bool? CanDamage() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>(Texture).Value;
            if (TimeSinceSpawn > 5)
            {
                Main.EntitySpriteDraw(sprite, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + player.fullRotation + MathHelper.PiOver2 + MathHelper.ToRadians(Main.rand.NextFloat(4f, 4f) * Recoil), sprite.Size() * 0.5f, 1f,
                    SpriteEffects.None);
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (player.HeldItem.type == ModContent.ItemType<CreanStaff>() && !player.GetModPlayer<TranscendencePlayer>().CannotUseItems && !player.dead)
                Projectile.timeLeft = 5;
            else return;

            Vector2 pos = player.Center + Projectile.velocity * 5.5f;
            Projectile.Center = pos;
            player.heldProj = Projectile.whoAmI;
            TimeSinceSpawn++;

            if (Recoil > 0f)
                Recoil = MathHelper.Lerp(Recoil, 0f, 1f / 5f);

            if (TimeSinceSpawn < 5)
                Projectile.ai[1] = -1;

            float rotation = (player.Center - Main.MouseWorld).ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4 - player.fullRotation;

            Projectile.velocity = player.DirectionTo(Main.MouseWorld) * (7.5f - Recoil);
            Projectile.rotation = rotation;

            Vector2 vec = Projectile.Center + Projectile.velocity * 4f;
            Vector2 vel = Projectile.DirectionTo(Main.MouseWorld);



            int snatcher = ModContent.ProjectileType<CreanSnatcher>();
            int cog = ModContent.ProjectileType<CreanCog>();
            int stargazer = ModContent.ProjectileType<CreanStargazer>();

            if (player.ownedProjectileCounts[snatcher] == 0 && Downed.Contains(Bosses.FrostSerpent))
            {
                for (int i = 0; i < 5; i++)
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, snatcher, player.HeldItem.damage / 4, 2f, player.whoAmI, 0f, i, 5f);
            }
            if (player.ownedProjectileCounts[cog] == 0 && Downed.Contains(Bosses.ProjectNucleus))
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, vel * 8f, cog, player.HeldItem.damage, 3f, player.whoAmI);
            }
            if (player.ownedProjectileCounts[stargazer] < 2 && Downed.Contains(Bosses.CelestialSeraph))
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, stargazer, player.HeldItem.damage, 2f, player.whoAmI, 0f, player.ownedProjectileCounts[stargazer], 2f);
            }

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation - MathHelper.PiOver4);
            dir = Main.MouseWorld.X > player.Center.X ? 1 : -1;
            player.direction = dir;


            if (Projectile.ai[2] > 0)
                Projectile.ai[2]--;

            if (player.controlUseItem && Projectile.ai[2] == 0)
            {
                for (int i = 0; i < 3; i++)
                SoundEngine.PlaySound(SoundID.Item72 with { Volume = 2, MaxInstances = 0}, Projectile.Center);

                int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.DirectionTo(Main.MouseWorld),
                    ModContent.ProjectileType<CreanStargazerLaser>(), Projectile.damage, 2f, Projectile.owner, 0, Projectile.whoAmI, 1f);
                Main.projectile[p].localAI[2] = Main.rand.Next(0, 40);


                if (player.HeldItem.GetGlobalItem<ModifiersItem>().ChargerCharge > 0)
                {
                    player.HeldItem.GetGlobalItem<ModifiersItem>().ChargerCharge -= 0.05f;
                    player.HeldItem.GetGlobalItem<ModifiersItem>().ChargeCD = 60;
                }

                if (player.GetModPlayer<TranscendencePlayer>().CreanStaffClick == 0)
                    player.GetModPlayer<TranscendencePlayer>().CreanStaffClick = 40;

                Recoil = 3f;
                Projectile.ai[1]++;
                Projectile.ai[2] = player.HeldItem.useAnimation;
            }
        }
    }
}