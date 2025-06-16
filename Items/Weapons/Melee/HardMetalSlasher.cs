using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Weapons;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class HardMetalSlasher : ModItem
    {
        int projectile = ModContent.ProjectileType<HardMetalSlasherProj>();
        int Cycle;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.DamageType = DamageClass.Melee;

            Item.width = 18;
            Item.height = 18;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Rapier;

            Item.knockBack = 4;
            Item.shoot = projectile;
            Item.shootSpeed = 13;

            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Green;

            Item.GetGlobalItem<ModifiersItem>().BlacksmithGiantHandleAllowed = true;

            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        public override bool CanShoot(Player player)
        {
            if (player.ownedProjectileCounts[projectile] > 0)
                return false;
            else return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position,
            ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => type = ProjectileID.None;
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[projectile] > 0)
                return false;
            else return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[projectile] < 1)
            {
                if (Cycle > 4)
                    Cycle = 0;
                Cycle++;
                Projectile.NewProjectile(source, position, velocity, projectile, damage, knockback, player.whoAmI, 0, Cycle > 4 ? 1 : 0, 12);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<HardmetalBar>(), 10)
             .AddRecipeGroup(nameof(ItemID.SilverBar), 15)
             .AddTile(TileID.Anvils)
             .Register();
        }
    }
    public class HardMetalSlasherProj : ModProjectile
    {
        public float vel = 0;

        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/HardMetalSlasher";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DismountsPlayersOnHit[Type] = true;
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 25;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<HardmetalPoisoning>(), 120);
            TranscendenceUtils.ParticleOrchestra(ParticleOrchestraType.SilverBulletSparkle, Projectile.Center + Projectile.velocity * 2, player.whoAmI);

            if (Projectile.ai[1] == 1)
            {
                target.SimpleStrikeNPC(damageDone * 5, hit.HitDirection, hit.Crit, hit.Knockback, hit.DamageType, !Main.player[Projectile.owner].GetModPlayer<TranscendencePlayer>().Eternity, player.luck, false);
                player.addDPS(damageDone * 5);

                SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
                Projectile.ai[1] = 0;

                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDustPerfect(target.Center, ModContent.DustType<HardmetalDust>(), Main.rand.NextVector2Circular(2.5f, 5f));
                    Dust.NewDustPerfect(target.Center, DustID.SilverCoin, Main.rand.NextVector2Circular(2.5f, 5f));

                }
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            if (player.HeldItem.type != ModContent.ItemType<HardMetalSlasher>() || player.dead)
                Projectile.Kill();

            Projectile.scale += MathHelper.Lerp(0.01f, 0.33f, 1f / 18f);

            if (Projectile.ai[1] == 1)
                Projectile.penetrate = 2;

            Projectile.Center = player.MountedCenter + (Projectile.velocity * vel * Projectile.scale);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            if (Projectile.timeLeft < 16 && Projectile.timeLeft > 6)
                vel = 2f;
            else vel += 0.25f;

            if (Projectile.timeLeft < 6)
            {
                vel -= 0.5f;
                if (Projectile.Distance(player.Center) < 5)
                    Projectile.Kill();
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * (3 + vel) * Projectile.scale * 0.75f, 20, ref reference))
            {
                return true;
            }
            else return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}