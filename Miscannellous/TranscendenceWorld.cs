using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using TranscendenceMod.Items.Armor.Hats;
using TranscendenceMod.Items.Consumables.Boss;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Consumables.Placeables.Decorations;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.NPCs.Miniboss;
using TranscendenceMod.Tiles.TilesheetHell.Nature;
using TranscendenceMod.Buffs;
using TranscendenceMod.Items.Farming;
using TranscendenceMod.NPCs.Boss.Seraph;
using TranscendenceMod.NPCs.Boss.FrostSerpent;
using Terraria.UI.Chat;
using Terraria.GameContent;
using TranscendenceMod.NPCs.Boss.Dragon;
using Terraria.GameContent.Liquid;
using TranscendenceMod.Tiles.BigTiles;
using TranscendenceMod.Tiles.BigTiles.Furniture;
using TranscendenceMod.NPCs.Boss.Nucleus;
using TranscendenceMod.Miscanellous.MiscSystems;
using TranscendenceMod.Items;
using TranscendenceMod.Items.Materials.MobDrops;

namespace TranscendenceMod
{
    public class TranscendenceWorld : ModSystem
    {
        public static ModKeybind RetLensKeybind;
        public static ModKeybind Guard;
        public static ModKeybind InfectionAccessoryKeyBind;
        public static ModKeybind HyperDash;
        public static ModKeybind ArmorSetBonus;

        public static bool EncouteredSeraph;
        public static bool EncouteredAtmospheron;

        public enum Bosses : byte
        {
            /*Just in case*/ _, 
            Muramasa,
            FrostSerpent,
            Atmospheron,
            ProjectNucleus,
            CelestialSeraph
        }
        public static List<Bosses> Downed = new List<Bosses>();

        public static bool ObtainedTimeDial;

        public static float CosmosColorFadeTimer;
        public static float CosmosColorFade = 0.01f;
        public static Color CosmicPurple;

        public static int sx = Main.maxTilesX - 1000;
        public static int sy = 135;
        public static int spy = 300;

        public static bool BoulderRain;
        public static int BoulderRainTime;

        public static float UniversalRotation;
        public static int Timer;

        public static int SpaceTempleX;
        public static Vector2[] seraphStarsPos = new Vector2[151];

        public static int VoidTilesCount;

        public static double PreTime;
        public static bool PreDay;

        public static bool AnyProjectiles(int Type)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].type == Type && Main.projectile[i].active)
                {
                    //Main.LocalPlayer.Center = Main.projectile[i].Center;
                    return true;
                }
            }
            return false;
        }

        public static bool SeraphArenaActive()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i] != null && Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<CelestialSeraph>() && Main.npc[i].ModNPC is CelestialSeraph boss)
                {
                    return boss.HasArena;
                }
            }
            return false;
        }

        public static int CountProjectiles(int Type)
        {
            int amount = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i] != null && Main.projectile[i].active && Main.projectile[i].type == Type)
                {
                    amount++;
                }
            }
            return amount;
        }

        public static void IntiateBoulderRain()
        {
            Main.NewText(Language.GetTextValue("Mods.TranscendenceMod.Messages.BoulderRainStart"), 175, 75, 255);
            BoulderRain = true;
            BoulderRainTime = (int)Main.dayLength / 4;
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n != null && n.active && n.ModNPC is ProjectNucleus boss)
                {
                    tileColor *= 1.5f;
                    backgroundColor *= 1.5f;

                    tileColor = Color.Lerp(tileColor, new Color(100, 10, 30), boss.RedAlpha / 3f);
                    tileColor = Color.Lerp(tileColor, new Color(0, 50, 80), boss.BlueAlpha / 3f);
                    tileColor = Color.Lerp(tileColor, new Color(80, 80, 80), boss.GrayAlpha / 3f);

                    backgroundColor = Color.Lerp(backgroundColor, new Color(100, 10, 30), boss.RedAlpha / 3f);
                    backgroundColor = Color.Lerp(backgroundColor, new Color(0, 50, 80), boss.BlueAlpha / 3f);
                    backgroundColor = Color.Lerp(backgroundColor, new Color(80, 80, 80), boss.GrayAlpha / 3f);
                }
            }

            if (TranscendenceUtils.BossAlive() && !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<ProjectNucleus>()))
                backgroundColor *= 1.5f;

            float fade = Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().NullFade;
            Color col = new Color(0, 255, 155);

            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ZoneStar && !Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().ZoneLimbo)
            {
                fade = Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().StarFade;
                col = Color.Magenta * 0.66f;
            }

            if (fade > 0)
            {
                float mult = 0.5f;
                if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().HasJellyBuff)
                    mult = 0.75f;

                tileColor = Color.Lerp(tileColor, col * mult, fade);
                backgroundColor = Color.Lerp(tileColor, col * mult, fade);
            }
        }

        public override void PostUpdateTime()
        {
            base.PostUpdateTime();

            if (TranscendenceUtils.BossAlive())
            {
                Main.time = PreTime;
                Main.dayTime = PreDay;
            }
        }
        public override void PreUpdateTime()
        {
            base.PreUpdateTime();

            PreTime = Main.time;
            PreDay = Main.dayTime;

            if (!Main.dayTime && Main.time > (Main.nightLength - 1))
            {
                if (!Main.dayTime && Main.snowMoon)
                {
                    Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer mp);
                    if (mp.FrostMoonHS < NPC.totalInvasionPoints)
                        mp.FrostMoonHS = (int)NPC.totalInvasionPoints;
                }

                bool Legendary = Main.getGoodWorld || Main.zenithWorld;
                if (Main.rand.NextBool(4) && Legendary)
                    IntiateBoulderRain();
            }
        }

        public void UpdateVoidTilesCount()
        {
            int a = 0;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile != null && tile.HasTile && tile.TileType == (ushort)ModContent.TileType<VoidTile>())
                        a++;
                }
            }
            
            VoidTilesCount = a;
        }

        public override void PostUpdateWorld()
        {
            SpaceTempleX = (int)(Main.maxTilesX / 3.75f) * 16;
            UniversalRotation += MathHelper.ToRadians(1);
            Timer++;

            if (Timer % 900 == 0)
                UpdateVoidTilesCount();

            if (BoulderRain)
            {
                BoulderRainTime--;
                if (Main.rand.NextBool(10))
                {
                    int type = Main.rand.NextBool(2) ? ProjectileID.MiniBoulder : ProjectileID.Boulder;
                    Projectile.NewProjectile(Main.LocalPlayer.GetSource_FromAI(), new Vector2(Main.LocalPlayer.Center.X + Main.rand.Next(-1000, 1000), 705), new Vector2(0, 10), type, 40, 2);
                }
            }
            if ((BoulderRainTime < 1 || Main.CurrentFrameFlags.AnyActiveBossNPC) && BoulderRain)
            {
                Main.NewText(Language.GetTextValue("Mods.TranscendenceMod.Messages.BoulderRainEnd"), 175, 75, 255);
                BoulderRain = false;
            }

            CosmosColorFadeTimer += CosmosColorFade;
            if (CosmosColorFadeTimer > 1 || CosmosColorFadeTimer < 0) CosmosColorFade = -CosmosColorFade;
            CosmicPurple = Color.Lerp(new Color(0f, 0.25f, 0.75f), new Color(0.75f, 0f, 0.65f), CosmosColorFadeTimer);
        }
        public override void PostSetupContent()
        {
            BossChecklistCrossMod();
        }

        public override void PostDrawTiles()
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Player p = Main.LocalPlayer;
            Main.LocalPlayer.TryGetModPlayer(out TranscendencePlayer mp);

            spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < mp.PortalBoxPositions.Length; i++)
                {
                    if (mp.PortalBoxPositions[i] != Vector2.Zero && p.HasItem(ModContent.ItemType<PortalBox>()))
                    {
                        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, (i + 1).ToString(), mp.PortalBoxPositions[i] + new Vector2(0, 4 + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 12f) * 2f) - FontAssets.MouseText.Value.MeasureString((i + 1).ToString()) * 0.5f - Main.screenPosition, Color.White, 0f, Vector2.Zero, Vector2.One);
                    }
                }
            }

            spriteBatch.End();

            base.PostDrawTiles();
        }
        public override void AddRecipes()
        {
            Recipe leather = Recipe.Create(ItemID.Leather);
            leather.AddIngredient(ItemID.Vertebrae, 5);
            leather.AddTile(TileID.WorkBenches);
            leather.Register();

            Recipe america = Recipe.Create(ItemID.Fries);
            america.AddIngredient(ModContent.ItemType<Potato>(), 8);
            america.AddTile(TileID.Furnaces);
            america.Register();

            Recipe chips = Recipe.Create(ItemID.PotatoChips);
            chips.AddIngredient(ModContent.ItemType<Potato>(), 12);
            chips.AddTile(TileID.Furnaces);
            chips.Register();

            Recipe pizza = Recipe.Create(ItemID.Pizza);
            pizza.AddIngredient(ModContent.ItemType<Flour>(), 8);
            pizza.AddIngredient(ModContent.ItemType<Tomato>(), 8);
            pizza.AddTile(TileID.Furnaces);
            pizza.Register();

            Recipe cookies = Recipe.Create(ItemID.ChocolateChipCookie, 2);
            cookies.AddIngredient(ModContent.ItemType<Flour>(), 4);
            cookies.AddIngredient(ModContent.ItemType<CocoaBean>(), 6);
            cookies.AddTile(TileID.Furnaces);
            cookies.Register();

            Recipe spaghetti = Recipe.Create(ItemID.Spaghetti);
            spaghetti.AddIngredient(ModContent.ItemType<Flour>(), 8);
            spaghetti.AddIngredient(ModContent.ItemType<Tomato>(), 8);
            spaghetti.AddTile(TileID.Furnaces);
            spaghetti.Register();

            Recipe spike = Recipe.Create(ItemID.Spike, 5);
            spike.AddRecipeGroup(RecipeGroupID.IronBar, 8);
            spike.AddTile(TileID.HeavyWorkBench);
            spike.DisableDecraft();
            spike.Register();

            Recipe Wspike = Recipe.Create(ItemID.WoodenSpike, 5);
            Wspike.AddIngredient(ItemID.RichMahogany, 3);
            Wspike.AddIngredient(ItemID.Bone, 3);
            Wspike.AddTile(TileID.HeavyWorkBench);
            Wspike.AddDecraftCondition(Condition.DownedSkeletron);
            Wspike.Register();

            Recipe dartTrap = Recipe.Create(ItemID.DartTrap);
            dartTrap.AddIngredient(ItemID.StoneSlab, 10);
            dartTrap.AddIngredient(ItemID.Stinger, 2);
            dartTrap.AddTile(TileID.HeavyWorkBench);
            dartTrap.Register();

            Recipe geyser = Recipe.Create(ItemID.GeyserTrap);
            geyser.AddIngredient(ItemID.StoneBlock, 15);
            geyser.AddIngredient(ModContent.ItemType<VolcanicRemains>(), 2);
            geyser.AddTile(TileID.HeavyWorkBench);
            geyser.DisableDecraft();
            geyser.Register();

            Recipe spearTrap = Recipe.Create(ItemID.SpearTrap);
            spearTrap.AddIngredient(ItemID.VenomDartTrap);
            spearTrap.AddIngredient(ItemID.LunarTabletFragment, 8);
            spearTrap.AddIngredient(ItemID.ChlorophyteBar, 5);
            spearTrap.AddTile(TileID.LihzahrdFurnace);
            spearTrap.DisableDecraft();
            spearTrap.Register();

            Recipe superDartTrap = Recipe.Create(ItemID.SuperDartTrap);
            superDartTrap.AddIngredient(ItemID.VenomDartTrap);
            superDartTrap.AddIngredient(ItemID.LunarTabletFragment, 8);
            superDartTrap.AddIngredient(ItemID.VialofVenom, 3);
            superDartTrap.AddTile(TileID.LihzahrdFurnace);
            superDartTrap.DisableDecraft();
            superDartTrap.Register();

            Recipe flameTrap = Recipe.Create(ItemID.FlameTrap);
            flameTrap.AddIngredient(ItemID.VenomDartTrap);
            flameTrap.AddIngredient(ItemID.LunarTabletFragment, 8);
            flameTrap.AddIngredient(ModContent.ItemType<CarbonOre>(), 4);
            flameTrap.AddTile(TileID.LihzahrdFurnace);
            flameTrap.DisableDecraft();
            flameTrap.Register();

            Recipe spikeTrap = Recipe.Create(ItemID.SpikyBallTrap);
            spikeTrap.AddIngredient(ItemID.VenomDartTrap);
            spikeTrap.AddIngredient(ItemID.LunarTabletFragment, 8);
            spikeTrap.AddIngredient(ItemID.SpikyBall, 14); 
            spikeTrap.AddTile(TileID.LihzahrdFurnace);
            spikeTrap.DisableDecraft();
            spikeTrap.Register();

            Recipe powerCell = Recipe.Create(ItemID.LihzahrdPowerCell);
            powerCell.AddIngredient(ModContent.ItemType<SunburntAlloy>(), 6);
            powerCell.AddIngredient(ItemID.Glass, 8);
            powerCell.AddIngredient(ItemID.Ectoplasm, 4);
            powerCell.AddTile(TileID.LihzahrdFurnace);
            powerCell.DisableDecraft();
            powerCell.Register();

            Recipe goldKey = Recipe.Create(ItemID.GoldenKey);
            goldKey.AddIngredient(ItemID.Bone, 20);
            goldKey.AddIngredient(ItemID.GoldOre, 2);
            goldKey.DisableDecraft();
            goldKey.Register();

            Recipe platKey = Recipe.Create(ItemID.GoldenKey);
            platKey.AddIngredient(ItemID.Bone, 20);
            platKey.AddIngredient(ItemID.PlatinumOre, 2);
            platKey.DisableDecraft();
            platKey.Register();
        }
        public override void PostAddRecipes()
        {
            for (int r = 0; r < Recipe.numRecipes; r++)
            {
                Recipe recipe = Main.recipe[r];

                if (recipe.createItem.type == ItemID.Fertilizer)
                    recipe.RemoveIngredient(ItemID.Bone);

                if (recipe.createItem.type == ItemID.HellstoneBar)
                {
                    recipe.RemoveIngredient(ItemID.Obsidian);
                    recipe.AddIngredient(ModContent.ItemType<VolcanicRemains>());
                }

                //Tier-shift Zenith to Post-Seraph
                if (recipe.createItem.type == ItemID.Zenith)
                {
                    recipe.RemoveTile(TileID.MythrilAnvil);
                    recipe.AddIngredient(ModContent.ItemType<StarcraftedAlloy>(), 4);
                    recipe.AddTile(ModContent.TileType<StarcraftedForge>());
                }

                if (recipe.HasResult(ItemID.LunarBar))
                    recipe.DisableRecipe();

                if (recipe.HasResult(ItemID.UnholyArrow) && recipe.HasIngredient(ItemID.Vertebrae))
                    recipe.DisableRecipe();
            }
        }
        public override void AddRecipeGroups()
        {
            if (RecipeGroup.recipeGroupIDs.ContainsKey("Wood"))
            {
                int wood = RecipeGroup.recipeGroupIDs["Wood"];
                RecipeGroup woodGroup = RecipeGroup.recipeGroups[wood];
                woodGroup.ValidItems.Add(ModContent.ItemType<CaveWood>());
            }

            RecipeGroup Gold = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldBar)}", ItemID.GoldBar, ItemID.PlatinumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBar), Gold);

            RecipeGroup MythrilAnvil = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37") + " " + Language.GetTextValue("Mods.TranscendenceMod.Items.HardmodeAnvil")}", ItemID.MythrilAnvil, ItemID.OrichalcumAnvil);
            RecipeGroup.RegisterGroup(nameof(ItemID.MythrilAnvil), MythrilAnvil);

            RecipeGroup AdamForge = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37") + " " + Language.GetTextValue("Mods.TranscendenceMod.Items.HardmodeForge")}", ItemID.AdamantiteForge, ItemID.TitaniumForge);
            RecipeGroup.RegisterGroup(nameof(ItemID.AdamantiteForge), AdamForge);

            RecipeGroup HollowHelms = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Hallowed Helmet",
            ItemID.HallowedMask, ItemID.HallowedHelmet, ItemID.HallowedHeadgear, ItemID.HallowedHood,
            ItemID.AncientHallowedMask, ItemID.AncientHallowedHelmet, ItemID.AncientHallowedHeadgear, ItemID.AncientHallowedHood);
            RecipeGroup.RegisterGroup(nameof(ItemID.HallowedMask), HollowHelms);

            RecipeGroup WhyIsntCopperAGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBar)}", ItemID.CopperBar, ItemID.TinBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.CopperBar), WhyIsntCopperAGroup);

            RecipeGroup WhyIsntSilverAGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.SilverBar)}", ItemID.SilverBar, ItemID.TungstenBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.SilverBar), WhyIsntSilverAGroup);

            RecipeGroup WhatAboutTitanium = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.TitaniumBar)}", ItemID.TitaniumBar, ItemID.AdamantiteBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.TitaniumBar), WhatAboutTitanium);

            RecipeGroup Shadowscale = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ShadowScale)}", ItemID.ShadowScale, ItemID.TissueSample);
            RecipeGroup.RegisterGroup(nameof(ItemID.ShadowScale), Shadowscale);
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int SpaceLand = tasks.FindIndex(genpass => genpass.Name.Equals("Surface Caves"));
            int SpaceBiome = tasks.FindIndex(genpass => genpass.Name.Equals("Spawn Point"));
            int Magnet = tasks.FindIndex(genpass => genpass.Name.Equals("Stalac"));
            int Caves = tasks.FindIndex(genpass => genpass.Name.Equals("Surface Caves"));
            int Structures = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            int Shinies2 = tasks.FindIndex(genpass => genpass.Name.Equals("Larva"));
            int SunkenCata = tasks.FindIndex(genpass => genpass.Name.Equals("Larva"));

            if (SpaceLand != -1)
                tasks.Insert(SpaceLand + 1, new CosmicValleyGenPass("SpaceLand", 550f));

            if (SpaceBiome != -1)
                tasks.Insert(SpaceBiome + 1, new SpaceBiomeGenPass("SpaceBiome", 550f));

            if (Magnet != -1)
                tasks.Insert(Magnet + 1, new CrateMagnets("Magnet", 550f));

            if (Caves != -1)
                tasks.Insert(Caves + 1, new CavinatorEX("Caves", 550f));

            if (Structures != -1)
                tasks.Insert(Structures + 1, new Structures("Structures", 550f));

            if (Shinies2 != -1)
                tasks.Insert(Shinies2 + 1, new Ores("Shinies2", 550f));

            if (SunkenCata != -1)
                tasks.Insert(SunkenCata + 1, new SunkenCatacombGen("SunkenCatacombs", 550f));
        }
        public override void ModifyLightingBrightness(ref float scale)
        {
            base.ModifyLightingBrightness(ref scale);

            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().IsBlind > 0)
                scale *= 0.925f;

            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().HasJellyBuff)
                scale += 0.04f;
        }
        private void BossChecklistCrossMod()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
                return;

            if (bossChecklistMod.Version < new Version(1, 6))
                return;

            Func<bool> muramasaDestroyed = () => Downed.Contains(Bosses.Muramasa);
            Func<bool> serpentSlain = () => Downed.Contains(Bosses.FrostSerpent);
            Func<bool> windDragonSlain = () => Downed.Contains(Bosses.Atmospheron);
            Func<bool> nucleusDestroyed = () => Downed.Contains(Bosses.ProjectNucleus);
            Func<bool> spaceBossSlain = () => Downed.Contains(Bosses.CelestialSeraph);

            List<int> nucleusDropTable = new List<int>()
                {
                    ModContent.ItemType<NucleusMask>(),
                    ModContent.ItemType<NucleusTrophyItem>(),
                    ModContent.ItemType<NucleusTrophyItem>(),
                };

            List<int> spaceBossDropTable = new List<int>()
                {
                    ModContent.ItemType<FaeMask>(),
                    ModContent.ItemType<FaeTrophyItem>(),
                    ModContent.ItemType<SeraphRelicItem>(),
                };

            var SpacePortrait = (SpriteBatch spriteBatch, Rectangle rec, Color color) =>
            {
                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Bestiary/CelestialSeraphBestiary").Value;
                Vector2 pos = new Vector2(rec.X + (rec.Width * 0.25f), rec.Y + (rec.Height * 0.25f));
                spriteBatch.Draw(sprite, new Rectangle((int)pos.X, (int)pos.Y, (int)(sprite.Width * 0.75f), (int)(sprite.Height * 0.75f)), color);
            };
            var FrostPortrait = (SpriteBatch spriteBatch, Rectangle rec, Color color) =>
            {
                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Bestiary/FrostSerpentBestiary").Value;
                Vector2 pos = new Vector2(rec.X + (rec.Width * 0.25f), rec.Y + (rec.Height / 2) - (sprite.Height / 2));
                spriteBatch.Draw(sprite, new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height), color);
            };
            var NucleusPortrait = (SpriteBatch spriteBatch, Rectangle rec, Color color) =>
            {
                Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/Assets/Bestiary/ProjectNucleus").Value;
                Vector2 pos = new Vector2(rec.X + (rec.Width / 2f) - (sprite.Width / 2), rec.Y + (rec.Height / 2) - (sprite.Height / 2));
                spriteBatch.Draw(sprite, new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height), color);
            };

            float muramasaProgression = 5.5f;
            float frostSerpentProgression = 18.125f;
            float windDragonProgression = 18.25f;
            float nucleusProgression = 18.85f;
            float spaceBossProgression = 20.25f;

            bossChecklistMod.Call("LogMiniBoss",
                Mod,
                "MuramasaBoss",
                muramasaProgression,
                muramasaDestroyed,
                ModContent.NPCType<MuramasaBoss>()
            );

            bossChecklistMod.Call("LogBoss",
                Mod,
                "FrostSerpent",
                frostSerpentProgression,
                serpentSlain,
                ModContent.NPCType<FrostSerpent_Head>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = new List<int> { ModContent.ItemType<HeartOfTheTundra>() },
                    ["customPortrait"] = FrostPortrait,
                }
            );

            bossChecklistMod.Call("LogBoss",
                Mod,
                "WindDragon",
                windDragonProgression,
                windDragonSlain,
                ModContent.NPCType<WindDragon>()
            );
            
            bossChecklistMod.Call("LogBoss",
                Mod,
                "ProjectNucleus",
                nucleusProgression,
                nucleusDestroyed,
                ModContent.NPCType<ProjectNucleus>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = new List<int> { ModContent.ItemType<NucleusSummonerItem>() },
                    ["collectibles"] = nucleusDropTable,
                    ["customPortrait"] = NucleusPortrait,
                }
            );

            bossChecklistMod.Call("LogBoss",
                Mod,
                "CelestialSeraph",
                spaceBossProgression,
                spaceBossSlain,
                ModContent.NPCType<CelestialSeraph>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = new List<int> { ModContent.ItemType<CosmicArtifact>() },
                    ["collectibles"] = spaceBossDropTable,
                    ["customPortrait"] = SpacePortrait,
                }
            );
        }
        public override void Load()
        {
            RetLensKeybind = KeybindLoader.RegisterKeybind(Mod, "Ret-125SModelLaserView", Microsoft.Xna.Framework.Input.Keys.J);
            Guard = KeybindLoader.RegisterKeybind(Mod, "Guard", Microsoft.Xna.Framework.Input.Keys.Z);
            InfectionAccessoryKeyBind = KeybindLoader.RegisterKeybind(Mod, "InfectionAccessoryKeyBind", Microsoft.Xna.Framework.Input.Keys.O);
            HyperDash = KeybindLoader.RegisterKeybind(Mod, "HyperDash", Microsoft.Xna.Framework.Input.Keys.LeftShift);
            ArmorSetBonus = KeybindLoader.RegisterKeybind(Mod, "ArmorSetBonus", Microsoft.Xna.Framework.Input.Keys.T);

            On_Main.DrawCursor += On_Main_DrawCursor;
            On_Main.DrawThickCursor += On_Main_DrawThickCursor;

            On_Player.GetItemGrabRange += On_Player_GetItemGrabRange;
            On_Player.AddBuff_DetermineBuffTimeToAdd += On_Player_AddBuff_DetermineBuffTimeToAdd;
            On_Player.GetAdjustedItemScale += On_Player_GetAdjustedItemScale;
            On_Main.DamageVar_float_int_float += On_Main_DamageVar_float_int_float;
            On_Projectile.AI_007_GrapplingHooks_CanTileBeLatchedOnTo += On_Projectile_AI_007_GrapplingHooks_CanTileBeLatchedOnTo;
            On_Projectile.GetFairyQueenWeaponsColor += On_Projectile_GetFairyQueenWeaponsColor;
            On_Item.CanShimmer += On_Item_CanShimmer;
        }

        private bool On_Item_CanShimmer(On_Item.orig_CanShimmer orig, Item self)
        {
            if (self.type == ItemID.RodofDiscord)
                return Downed.Contains(Bosses.CelestialSeraph);

            return orig(self);
        }

        private Vector2 On_Main_DrawThickCursor(On_Main.orig_DrawThickCursor orig, bool smart)
        {
            if (Main.LocalPlayer != null && Main.LocalPlayer.active && Main.LocalPlayer.TryGetModPlayer(out NucleusGame game) && game != null && game.Active)
                return Vector2.Zero;

            return orig(smart);
        }

        private void On_Main_DrawCursor(On_Main.orig_DrawCursor orig, Vector2 bonus, bool smart)
        {
            if (Main.LocalPlayer != null && Main.LocalPlayer.active && Main.LocalPlayer.TryGetModPlayer(out NucleusGame game) && game != null && game.Active)
                return;

            orig(bonus, smart);
        }

        private bool On_Projectile_AI_007_GrapplingHooks_CanTileBeLatchedOnTo(On_Projectile.orig_AI_007_GrapplingHooks_CanTileBeLatchedOnTo orig, Projectile self, int x, int y)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<CelestialSeraph>()))
            {
                if (SeraphArenaActive())
                {
                    for (int i = 0; i < 150; i++)
                    {
                        if (self != null && self.active && self.Distance(seraphStarsPos[i]) < 20)
                        {
                            self.Center = seraphStarsPos[i];
                            return true;
                        }
                    }
                }

                return false;
            }
            return orig(self, x, y);
        }

        private int On_Player_GetItemGrabRange(On_Player.orig_GetItemGrabRange orig, Player self, Item item)
        {
            int Range = orig(self, item);
            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().RaiderSetWear)
            {
                if (item.type == ItemID.CopperCoin || item.type == ItemID.SilverCoin || item.type == ItemID.GoldCoin || item.type == ItemID.PlatinumCoin)
                    return Range + 250;
                return Range + 125;
            }
            else return Range;
        }

        private Color On_Projectile_GetFairyQueenWeaponsColor(On_Projectile.orig_GetFairyQueenWeaponsColor orig, Projectile self, float alphaChannelMultiplier, float lerpToWhite, float? rawHueOverride)
        {
            Color col = orig(self, alphaChannelMultiplier, lerpToWhite, rawHueOverride);
            if (Main.player[self.owner].name.Contains("CreanBL"))
                return Color.Lerp(Color.Blue, Color.Aqua, (float)Math.Cos(Main.GlobalTimeWrappedHourly) * lerpToWhite);
            return col;
        }

        private int On_Main_DamageVar_float_int_float(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
        {
            if (Main.LocalPlayer.GetModPlayer<TranscendencePlayer>().Eternity)
                return orig(dmg, 0, luck);
            return orig(dmg, percent, luck);
        }

        private float On_Player_GetAdjustedItemScale(On_Player.orig_GetAdjustedItemScale orig, Player self, Item item)
        {
            float scale = orig(self, item);

            if (self.GetModPlayer<TranscendencePlayer>().SharkscaleSetWear && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed))
                scale += 0.375f;

            if (self.GetModPlayer<TranscendencePlayer>().UsingLunarGauntlet && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed))
                scale *= 1.25f;

            if (self.GetModPlayer<TranscendencePlayer>().BigHandle)
                scale *= 1.75f;

            return scale;
        }

        private int On_Player_AddBuff_DetermineBuffTimeToAdd(On_Player.orig_AddBuff_DetermineBuffTimeToAdd orig, Player self, int type, int time1)
        {
            int buffTime = orig(self, type, time1);

            if (!Main.debuff[type] && time1 > 30 && !Main.buffNoTimeDisplay[type] && self.GetModPlayer<TranscendencePlayer>().OcramHelmet)
                return buffTime *= 2;

            return buffTime;
        }

        public override void Unload()
        {
            Guard = null;
            InfectionAccessoryKeyBind = null;
            ArmorSetBonus = null;
        }
        public override void OnWorldLoad()
        {
            Downed.Clear();
            ObtainedTimeDial = false;
            EncouteredAtmospheron = false;
            EncouteredSeraph = false;
        }
        public override void OnWorldUnload()
        {
            Downed.Clear();
            ObtainedTimeDial = false;
            EncouteredAtmospheron = false;
            EncouteredSeraph = false;

            SkyManager.Instance.Deactivate("TranscendenceMod:CelestialSeraph");
            SkyManager.Instance.Deactivate("TranscendenceMod:SpaceMonolith");
            SkyManager.Instance.Deactivate("TranscendenceMod:DragonSky");
            if (Filters.Scene["TranscendenceMod:FlashbangShader"].IsActive()) Filters.Scene["TranscendenceMod:FlashbangShader"].Deactivate();
            if (Filters.Scene["TranscendenceMod:ScreenVignette"].IsActive()) Filters.Scene["TranscendenceMod:ScreenVignette"].Deactivate();
        }
        public override void SaveWorldData(TagCompound tag)
        {
            //tag.Add("Downed", Downed);
            if (ObtainedTimeDial) tag["ObtainedTimeDial"] = true;
            if (EncouteredSeraph) tag["EncouteredSeraph"] = true;
            if (VoidTilesCount > 0) tag["VoidTilesCount"] = VoidTilesCount;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            Downed = (List<Bosses>)tag.GetList<Bosses>("Downed");
            ObtainedTimeDial = tag.ContainsKey("ObtainedTimeDial");
            EncouteredAtmospheron = tag.ContainsKey("EncouteredAtmospheron");
            EncouteredSeraph = tag.ContainsKey("EncouteredSeraph");
            VoidTilesCount = tag.GetInt("VoidTilesCount");

        }
    }
}