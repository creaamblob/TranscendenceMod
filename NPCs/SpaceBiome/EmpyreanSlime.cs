using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.Biomes;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.NPCs.Bosses.SpaceBoss;

namespace TranscendenceMod.NPCs.SpaceBiome
{
    public class EmpyreanSlime : SpaceBiomeNPC
    {
        public int AttackTimer;
        public bool TouchedGround;
        public float Fade;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3;
            NPCID.Sets.TrailCacheLength[Type] = 20;
            NPCID.Sets.TrailingMode[Type] = 1;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = NPC.downedMoonlord ? 2505 : 380;
            NPC.defense = 10;
            NPC.damage = NPC.downedMoonlord ? 100 : 60;
            NPC.knockBackResist = NPC.downedMoonlord ? 0f : 0.25f;

            NPC.width = 48;
            NPC.height = 40;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.lavaImmune = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = false;
            NPC.value = Item.buyPrice(silver: 25);
            SpawnModBiomes = new int[2] { ModContent.GetInstance<CosmicDimensions>().Type,
                ModContent.GetInstance<Heaven>().Type };
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PulverizedPlanet>(), 2, 2, 4));
            npcLoot.Add(ItemDropRule.ByCondition(new MoonlordDropRule(), ItemID.FragmentNebula, 2, 2, 3));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<TranscendencePlayer>().ZoneStar && Main.hardMode)
                return 0.85f;
            else return 0;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/ExpandingTelegraph").Value;
            Vector2 pos = NPC.Center - Main.screenPosition;

            spriteBatch.Draw(sprite, new Rectangle((int)(pos.X - (NPC.ai[0] / 2)), (int)pos.Y, (int)NPC.ai[0], 2000), null, new Color(0.75f, 0.1f, 0.4f, 0f) * Fade);
            TranscendenceUtils.DrawTrailNPC(NPC, Color.Magenta, NPC.scale, Texture + "_Glow", false, true, 1.5f, new Vector2(0, 4));

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;

            NPC.rotation = NPC.velocity.X * 0.075f;
            NPC.velocity.Y += 0.25f;
            bool downedML = NPC.downedMoonlord;
            bool OnGround = Collision.SolidCollision(NPC.Left, NPC.width, NPC.height);

            NPC.noTileCollide = NPC.ai[3] > 0;

            if (NPC.Center.Y < (player.Center.Y - 125) && (OnGround || NPC.ai[3] > 0))
                NPC.ai[3]++;

            else NPC.ai[3] = 0;

            if (AttackTimer < 51 && OnGround)
            {
                NPC.ai[2] = NPC.direction;
                NPC.ai[0] = 0;
                Fade = 0;
                NPC.velocity.X *= 0.8f;
                AttackTimer++;
            }

            if (NPC.ai[1] > 0)
                NPC.ai[1]--;

            if (AttackTimer > 50)
            {
                TouchedGround = false;

                bool Crushing = (NPC.ai[1] > 0 && OnGround || NPC.Center.Between(player.Center - new Vector2(20, 2000), player.Center + new Vector2(20, -100)));
                if (NPC.Center.Between(player.Center - new Vector2(600, 1500), player.Center + new Vector2(600, 1500)) && NPC.downedMoonlord)
                {
                    if (NPC.ai[0] < NPC.width)
                    {
                        if (!Crushing && NPC.ai[0] > 30 && Fade > 0)
                            Fade -= 0.025f;

                        Fade += 0.0125f;
                        NPC.ai[0]++;
                    }
                }
                else Fade -= 0.025f;

                if (Crushing && downedML)
                {
                    if (!OnGround)
                    {
                        NPC.velocity.Y = 20;
                        NPC.ai[1] = 100;
                    }
                    else
                    {
                        for (int i = -50; i < 100; i += 50)
                        {
                            int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(i / 2f, 0), new Vector2(i / 20f, -2.5f), ModContent.ProjectileType<StellarFireball>(), 70, 2, -1, 0, 0, 0.25f);
                            Main.projectile[p].friendly = false;
                        }
                        SoundEngine.PlaySound(SoundID.Item167, NPC.Center);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 500, 8, -1, 1f, 0.2f, 0.8f);

                        NPC.ai[1] = 0;
                        AttackTimer = 0;
                    }
                    NPC.velocity.X = 0;
                }
                else
                {
                    AttackTimer++;
                    if (NPC.ai[1] == 0)
                        NPC.velocity.X = (downedML ? 7.5f : 3.33f) * NPC.ai[2];
                    if (AttackTimer < 70)
                        NPC.velocity.Y = downedML ? -12.5f : -7.5f;
                }
            }
            if (AttackTimer > 150)
                AttackTimer = 0;
        }
        public override void FindFrame(int frameHeight)
        {
            if (!Collision.SolidCollision(NPC.Left, NPC.width, NPC.height))
            {
                NPC.frame.Y = 40;
                return;
            }

            if (NPC.frame.Y < 80)
            {
                if (++NPC.frameCounter > 5)
                {
                    NPC.frame.Y += frameHeight;
                    NPC.frameCounter = 0;
                }
            }
            else NPC.frame.Y = 0;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<SpaceDebuff>(), 120);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TranscendenceMod.Messages.Bestiary.EmpyreanSlime")),
            });
        }
        public override bool? CanFallThroughPlatforms() => true;
    }
}

