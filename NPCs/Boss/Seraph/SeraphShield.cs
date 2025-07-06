using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Projectiles;
using TranscendenceMod.Projectiles.Weapons;
using TranscendenceMod.Projectiles.Weapons.Melee;
using TranscendenceMod.Projectiles.Weapons.Ranged;

namespace TranscendenceMod.NPCs.Boss.Seraph
{
    public class SeraphShield : ModNPC
    {
        public int AttackDelay;
        public bool GrandFinale;
        public bool NoCollision;
        CelestialSeraph boss;
        NPC npc;
        public override string Texture => "TranscendenceMod/Miscannellous/Assets/InvisSprite";

        public override void SetStaticDefaults() => TranscendenceUtils.BeGoneBestiary(NPC);
        public override void SetDefaults()
        {
            NPC.lifeMax = 200000;
            NPC.defense = 65;
            NPC.takenDamageMultiplier = 0.5f;
            NPC.knockBackResist = 0f;
            NPC.GetGlobalNPC<TranscendenceNPC>().Unmovable = true;

            NPC.width = 185;
            NPC.height = 185;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;

            NPC.HitSound = new SoundStyle("TranscendenceMod/Miscannellous/Assets/Sounds/NPCs/Hurt/CelestialSeraphShield")
            {
                Volume = 1.2f,
                Pitch = 2f,
                MaxInstances = 0
            };
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.friendly = false;
        }
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCsOverPlayers.Add(index);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            npc = Main.npc[(int)NPC.ai[1]];
            if (npc.ModNPC is CelestialSeraph boss)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                string spritepath = $"Terraria/Images/Extra_193";
                Texture2D sprite = ModContent.Request<Texture2D>(spritepath).Value;
                /*Spherical Shield*/
                //Vector2 shieldPos = NPC.Center + new Vector2(5, 100) - Main.screenPosition;
                //Rectangle rec = new Rectangle(0, 0, 500, 320);
               // int expansion = (int)(Math.Sin(TranscendenceWorld.UniversalRotation * 3f) * 25);
                Vector2 shieldPos = NPC.Center - Main.screenPosition;
                Rectangle rec = new Rectangle(0, 0, 900, 690);

                float alpha = MathHelper.Lerp(0f, 1f, NPC.life / ((float)NPC.lifeMax * 0.33f));
                alpha -= 1f;

                if (boss.Attack != SeraphAttacks.NebulaMatter)
                {
                    if (npc != null && npc.ModNPC is CelestialSeraph boss2)
                    {
                        Color col = npc.ai[1] < 2 ? Color.Transparent : Color.Lerp(new Color(1f, 0.4f, 0f), new Color(1f, 1f, 1f), (float)Math.Sin(TranscendenceWorld.UniversalRotation * 2f)) * 2f * boss2.NPCFade * alpha;
                        DrawData drawData = new DrawData(sprite, shieldPos, rec, col, 0, rec.Size() * 0.5f,
                            NPC.scale, SpriteEffects.None);

                        GameShaders.Misc["ForceField"].UseColor(col);
                        GameShaders.Misc["ForceField"].Apply(drawData);
                        drawData.Draw(spriteBatch);
                    }
                }

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.NebulaPickup1));
            npcLoot.Add(ItemDropRule.Common(ItemID.NebulaPickup2));
            npcLoot.Add(ItemDropRule.Common(ItemID.NebulaPickup3));
        }
        public override void AI()
        {
            npc = Main.npc[(int)NPC.ai[1]];
            NPC.Center = npc.Center - new Vector2(0, NPC.ai[3] != 1 ? 0 : npc.height * 0.33f);
            NPC.hide = true;


            if (NPC.life < (int)(NPC.lifeMax * 0.33f) && NPC.ai[3] != 1)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Shockwave>(), 4000, 40, -1, 255, 200, 0);
                NPC.HitSound = SoundID.NPCHit4;
                NPC.width = 48;
                NPC.height = 48;
                NPC.takenDamageMultiplier = 1f;
                NPC.ai[3] = 1;
            }

            if (++NPC.ai[2] > 5 && NPC.ai[2] < 35)
            {
                NPC.lifeMax = (int)(npc.lifeMax * (Main.masterMode ? 0.233f : Main.expertMode ? 0.266f : 0.285f));
                NPC.life = (int)(npc.lifeMax * (Main.masterMode ? 0.233f : Main.expertMode ? 0.266f : 0.285f));
            }

            if (npc.ModNPC is CelestialSeraph boss)
            {
                boss.ShieldLifeMax = NPC.lifeMax;
                boss.ShieldLife = NPC.life;
                NoCollision = NPC.dontTakeDamage || boss.Attack == SeraphAttacks.GalaxyShardDash || boss.Attack == SeraphAttacks.Supernova || boss.Attack == SeraphAttacks.StellarFirestorm || boss.Attack == SeraphAttacks.RoyalFlash;

                if (GrandFinale || npc.ai[1] < 2)
                    NPC.dontTakeDamage = true;
                else
                {
                    NPC.dontTakeDamage = false;
                   // if (local.Distance(NPC.Center) < (150 * NPC.scale) && !NoCollision && boss.Timer_AI > 60)
                        //local.velocity = NPC.DirectionTo(local.Center) * 12.5f;
                }

                if (!npc.active)
                    NPC.active = false;
            }
        }
        public override bool CheckActive() => false;
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.DamageType == DamageClass.Melee)
                modifiers.FinalDamage *= 1.15f;
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ModContent.ProjectileType<DeepwaterSlash>()) modifiers.FinalDamage *= 0.4f;
            if (projectile.type == ModContent.ProjectileType<ExoticRayBowTrailingShot>()) modifiers.FinalDamage *= 0.25f;
            if (projectile.type == ModContent.ProjectileType<VoltageBeam>()) modifiers.FinalDamage *= 0.66f;
            if ((projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed)
                && Main.player[projectile.owner].heldProj == projectile.whoAmI) modifiers.FinalDamage *= 1.15f;

            if (projectile.type == ProjectileID.FinalFractal) modifiers.FinalDamage *= 0.175f;
            if (projectile.type == ProjectileID.SolarWhipSwordExplosion) modifiers.FinalDamage *= 0.66f;
        }
        public override bool PreKill()
        {
            npc.dontTakeDamage = false;

            for (int i = 0; i < 7; i++)
            {
                Item.NewItem(NPC.GetSource_FromAI(), NPC.getRect(), ItemID.Heart);
            }
            return base.PreKill();
        }
    }
}

