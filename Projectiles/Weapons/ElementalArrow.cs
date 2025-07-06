using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Dusts;
using TranscendenceMod.Items.Weapons;
using TranscendenceMod.Miscannellous;

namespace TranscendenceMod.Projectiles.Weapons
{
    public class ElementalArrow : ModProjectile
    {
        public Vector2 startVel;
        public NPC npc;
        public int Element;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Main.projFrames[Type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Stake;

            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<MagicRangedDamageClass>();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            startVel = Projectile.Center - target.Center;
            npc = target;
            Projectile.velocity = Projectile.DirectionTo(npc.Center) * 0.0000001f;

            int buffID = ModContent.BuffType<EarthernFortification>();
            switch (Element)
            {
                case 0: buffID = ModContent.BuffType<EarthernFortification>(); break;
                case 1: buffID = BuffID.Wet; break;
                case 2: buffID = ModContent.BuffType<Incineration>(); break;
                case 3: buffID = BuffID.Confused; break;
            }
            Vector2 pos = target.Center - new Vector2(Main.rand.Next(-210, 210), 1000);
            if (Element != 4)
                target.AddBuff(buffID, 180);
            else Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, pos.DirectionTo(target.Center) * 15f, ProjectileID.FallingStar, Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item4);
            Element = Main.rand.Next(0, 5);
            Projectile.frame = Element;
        }
        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item4);
            for (int t = 0; t < 15; t++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Rainbow>(), Main.rand.NextVector2CircularEdge(1f, 1f) * 4, 0, Main.DiscoColor, 1.75f);
                dust.noGravity = true;

            }
            return base.PreKill(timeLeft);
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;

            if (npc != null)
            {
                Projectile.Center = npc.Center + startVel;
                if (!npc.active)
                    Projectile.Kill();
            }
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool PreDraw(ref Color lightColor)
        {
            if (npc == null)
            {
                TranscendenceUtils.DrawTrailProj(Projectile, Color.White, 1f, "TranscendenceMod/Miscannellous/Assets/Trail2", true, true, 2f, Vector2.Zero);
            }
            TranscendenceUtils.DrawProjAnimated(Projectile, Color.White, Projectile.scale, $"{Texture}", Projectile.rotation, Projectile.Center, false, false, false);
            return false;
        }
    }
}