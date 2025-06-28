using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Melee;

namespace TranscendenceMod.Items.Weapons.Melee
{
    public class DarkMetalClaymore : ModItem
    {
        public int State = 1;
        public int proj = ModContent.ProjectileType<DarkMetalClaymoreProj>();

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 445;
            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.width = 24;
            Item.height = 24;

            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Rapier;

            Item.knockBack = 4.25f;
            Item.shoot = proj;
            Item.shootSpeed = 6;

            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<ModdedPurple>();

            Item.GetGlobalItem<ModifiersItem>().BlacksmithGiantHandleAllowed = true;

            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position,
            ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => type = ProjectileID.None;
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<DarkMetalClaymoreProj>()] > 0)
                return false;
            else return base.CanUseItem(player);
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[proj] < 1)
            {
                float sizeMult = player.GetAdjustedItemScale(Item);
                Projectile.NewProjectile(source, position, new Vector2(-5f * player.direction, -2.5f), proj, damage, knockback, -1, 0, sizeMult, 12);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
             .AddIngredient(ModContent.ItemType<RigidBroadsword>())
             .AddIngredient(ModContent.ItemType<VoidFragment>(), 12)
             .AddTile(TileID.MythrilAnvil)
             .Register();
        }
    }
    internal class DarkMetalClaymoreProj : ModProjectile
    {
        public override string Texture => "TranscendenceMod/Items/Weapons/Melee/DarkMetalClaymore";

        public float Timer;
        public float SparkleTimer;
        public float RotSpeed = 0.4f;
        public bool boolean;
        public int orb = ModContent.ProjectileType<ClaymoreOrb>();

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 68;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;

            Projectile.ownerHitCheck = true;
            Projectile.netImportant = true;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 32;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Projectile.DirectionTo(target.Center) * 8f, orb, hit.Damage, hit.Knockback, Main.player[Projectile.owner].whoAmI);
            Projectile.damage = (int)(Projectile.damage * 0.75f);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[2] = Main.player[Projectile.owner].direction;
            Timer = MathHelper.ToRadians(5) * Projectile.ai[2];
            Projectile.scale = Projectile.ai[1] * 1.25f;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float reference = float.NaN;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + Projectile.velocity * 8f * Projectile.scale, 16, ref reference) && Projectile.timeLeft < 22)
            {
                return true;
            }
            else return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.MountedCenter - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            if (player.HeldItem.type != ModContent.ItemType<DarkMetalClaymore>() || player.dead)
                Projectile.Kill();

            Projectile.Center = player.MountedCenter + (Projectile.velocity * 2f * (Projectile.scale * 3f));
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            Projectile.velocity = Projectile.velocity.RotatedBy(Timer / 60f);

            if (Projectile.timeLeft < 22)
            {
                RotSpeed = MathHelper.Lerp(RotSpeed, 1.8f, 0.125f);
                Timer += RotSpeed * Projectile.ai[2];
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>($"{Texture}").Value;

            Vector2 origin = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            float rot = Projectile.rotation + (Projectile.ai[2] > 0 ? 0 : MathHelper.PiOver2);
            SpriteEffects spriteEffects = Projectile.ai[2] > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(sprite, Projectile.Center + Projectile.velocity * 2.85f * Projectile.scale - Main.screenPosition, null, lightColor, rot, origin,
                Projectile.scale, spriteEffects);
            return false;
        }
    }
}