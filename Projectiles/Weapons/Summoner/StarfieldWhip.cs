using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.Weapons;
using TranscendenceMod.Dusts;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.Projectiles.Weapons.Summoner
{
    public class StarfieldWhip : ModProjectile
    {
        public int star = ModContent.ProjectileType<StarfieldStar>();
        public int ProjTimer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 40;
            Projectile.WhipSettings.RangeMultiplier = 1.925f;
        }
        public override void AI()
        {
            Vector2 pos = Projectile.WhipPointsForCollision[Main.rand.Next(Projectile.WhipPointsForCollision.Count - 1, Projectile.WhipPointsForCollision.Count)];
            int d = Dust.NewDust(Projectile.position, 1, 1, ModContent.DustType<Ember2>(), 0, 0, 0, Color.Blue, 0.75f);
            Main.dust[d].position = pos;
            Lighting.AddLight(pos, 2f, 0.5f, 0f);
            Projectile.ai[2]++;

            Player Owner = Main.player[Projectile.owner];
            if (pos.Distance(Owner.Center) > 100 && ++ProjTimer % 3 == 0)
            {
                int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<Meteor>(), (int)(Projectile.damage * 0.375f), 3f, Owner.whoAmI);
                Main.projectile[p].hostile = false;
                Main.projectile[p].friendly = true;
            }

            /*Main.instance.CameraModifiers.Add(new PunchCameraModifier(new Vector2(Main.rand.Next(-10, 10)),
                new Vector2(Main.rand.NextFloat()), ShakeIntensity, 4, 40, -1, null));*/
        }
        public override void PostDraw(Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);
            Main.DrawWhip_WhipMace(Projectile, list);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<StarfieldDebuff>(), 240);
        }
    }
}