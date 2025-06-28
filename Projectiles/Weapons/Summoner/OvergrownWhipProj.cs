using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Weapons;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class OvergrownWhipProj : ModProjectile
    {
        public int chain = ModContent.ProjectileType<OvergrownChain>();
        public bool Hit;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 20;
            Projectile.WhipSettings.RangeMultiplier = 1.4275f;
        }
        public override void AI()
        {
            Vector2 pos = Projectile.WhipPointsForCollision[Main.rand.Next(Projectile.WhipPointsForCollision.Count - 1, Projectile.WhipPointsForCollision.Count)];
            int d = Dust.NewDust(Projectile.position, 1, 1, DustID.Plantera_Green, 0, 0, 0, Color.White, 1f);
            Main.dust[d].position = pos;

            Player Owner = Main.player[Projectile.owner];
        }
        public override void PostDraw(Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);
            Main.DrawWhip_BoneWhip(Projectile, list);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<OvergrownDebuff>(), 240);
            if (Main.player[Projectile.owner].ownedProjectileCounts[chain] < 15 && !Hit)
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center - ((target.Center - Projectile.Center) / 4f), Vector2.Zero, chain, damageDone / 2, 0, Main.player[Projectile.owner].whoAmI);
            Hit = true;
        }
    }
}