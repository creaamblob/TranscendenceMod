using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using TranscendenceMod.Buffs.Items;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Equipment.Tools
{
    public class RodOfRelocationWarp : ModProjectile
    {
        public Player player;
        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 4;
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 450;

            Projectile.width = 36;
            Projectile.height = 36;

            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }
        public override bool PreKill(int timeLeft)
        {
            player.AddBuff(ModContent.BuffType<Distortion>(), 300);
            player.Teleport(Projectile.Top, TeleportationStyleID.RecallPotion);
            return base.PreKill(timeLeft);  
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            player = Main.player[Projectile.owner];

            if (player == null)
                return;

            Projectile.timeLeft = 99;

            if (Main.rand.NextBool(2))
            {
                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2FromRectangle(new Rectangle(-50, -100, 100, 200)), DustID.MagicMirror, new Vector2(0, -0.25f));
            }

            if (Main.mouseRight)
                Projectile.Kill();

            TranscendenceUtils.AnimateProj(Projectile, 5);
        }
    }
    public class RoRMap : ModMapLayer
    {
        public override void Draw(ref MapOverlayDrawContext context, ref string text)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj != null && proj.active && proj.type == ModContent.ProjectileType<RodOfRelocationWarp>())
                {
                    Texture2D icon = ModContent.Request<Texture2D>(TranscendenceMod.ASSET_PATH + "/Icons/Gateway").Value;

                    context.Draw(icon, proj.Center / 16, Color.White, new SpriteFrame(1, 1, 0, 0), 1f, 1f, Alignment.Center);

                    if (context.Draw(icon, proj.Center / 16, Alignment.Center).IsMouseOver)
                        text = Language.GetTextValue("Mods.TranscendenceMod.Items.RodOfRelocation.WarpPoint");
                }
            }
        }
    }
}