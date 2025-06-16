using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous.Rarities;
using TranscendenceMod.Projectiles.Weapons.Summoner;

namespace TranscendenceMod.Items.Weapons.Summoner
{
    public class Starfield : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 135;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.knockBack = 6;
            Item.crit = 5;

            Item.width = 16;
            Item.height = 24;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item1;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(gold: 35);
            Item.rare = ModContent.RarityType<MidnightBlue>();
            Item.shoot = ModContent.ProjectileType<StarfieldWhip>();
            Item.shootSpeed = 4f;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}