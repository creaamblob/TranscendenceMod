using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Dusts;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Magic;

namespace TranscendenceMod.Items.Weapons.Magic
{
    public class Constellations : ModItem
    {
        int proj = ModContent.ProjectileType<Constellations_HoldOut>();
        int proj2 = ModContent.ProjectileType<ConstellationsProj>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.damage = 185;
            Item.mana = 20;
            Item.knockBack = 5f;
            Item.crit = 15;
            Item.noUseGraphic = true;

            Item.width = 12;
            Item.height = 26;
            Item.UseSound = SoundID.Item60;

            Item.useTime = 5;
            Item.useAnimation = 30;
            Item.reuseDelay = 30;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.value = Item.sellPrice(gold: 35);
            Item.rare = ModContent.RarityType<MidnightBlue>();
            Item.shoot = proj2;
            Item.shootSpeed = 1f;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            TranscendenceUtils.DrawItemGlowmask(Item, rotation, scale, Texture);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = Vector2.Zero;
            position = Main.MouseWorld;
        }
        public override bool CanShoot(Player player)
        {
            // player.altFunctionUse != 2 && player.ownedProjectileCounts[proj2] < 15
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                int Counter = 0;
                if (p.type == proj2 && p.active && p.timeLeft < 30)
                    Counter++;
                return Counter < 2;
            }
            return player.altFunctionUse != 2 && player.ownedProjectileCounts[proj2] < 25;
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[proj] == 0)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero,
                    proj, Item.damage, Item.knockBack, player.whoAmI);
            }
        }
    }
    public class Constellations_HoldOut : ModProjectile
    {
        public float VisualRotationSpeed = 2.5f;
        public float Rotation = 0;
        public float RotationSpeed = 1.275f;
        public Vector2 Spin = new Vector2(0, 8);
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 28;

            Projectile.ignoreWater = true;
            Projectile.light = 0.15f;

            Projectile.penetrate = -1;
            Projectile.timeLeft = int.MaxValue;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, 1, 0.4f, 0.7f);

            if (player.HeldItem.type != ModContent.ItemType<Constellations>() || player.GetModPlayer<TranscendencePlayer>().CannotUseItems || player.dead)
                Projectile.Kill();

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, 80 * player.direction);
            Projectile.spriteDirection = -Projectile.direction;

            Projectile.Center = player.Center + new Vector2(45 * player.direction, 5);

            Rotation += VisualRotationSpeed;
            if (Rotation > 15 || Rotation < -15)
                VisualRotationSpeed = -VisualRotationSpeed;

            Dust.NewDustPerfect(Projectile.Center + new Vector2(Rotation, -5), ModContent.DustType<NovaDust>(), new Vector2(0, -5));
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            Main.instance.DrawCacheProjsOverPlayers.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < 4; i++)
            {
                float pi = MathHelper.TwoPi * i / 4;
                float rot = pi += MathHelper.ToRadians(++Projectile.localAI[2] + 4);

                Vector2 pos = Projectile.Center + Vector2.One.RotatedBy(rot) * 5;

                TranscendenceUtils.DrawProjAnimated(Projectile, Color.DeepSkyBlue * 0.1f, Projectile.scale * 1.25f,
                    $"{Texture}", Projectile.rotation, pos, false, false, false);
            }
            return true;
        }
    }
}